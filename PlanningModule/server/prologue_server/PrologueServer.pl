

:- dynamic availability/3.
:- dynamic agenda_staff/3.
:- dynamic agenda_staff1/3.
:-dynamic agenda_operation_room/3.
:-dynamic agenda_operation_room1/3.
:-dynamic better_sol/5.


:- dynamic agenda_staff/3.
:- dynamic surgery/4.
:- dynamic staff/4.
:- dynamic timetable/3.
:- dynamic surgery_id/2.
:- dynamic assignment_surgery/2.

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Inicia o servidor na porta especificada
iniciar_servidor(Porta) :-
    http_server(http_dispatch, [port(Porta)]).

% Inicialização automática do servidor
:- initialization(iniciar_servidor(8080)).

:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_json)).
:- use_module(library(http/http_parameters)).
:- use_module(library(lists)).
:- use_module(library(http/http_cors)).
:- use_module(library(http/http_server)).

% Configuração CORS
:- set_setting(http:cors, [ '*' ]).

:- set_prolog_flag(encoding, utf8).



%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Registra os handlers.
:- http_handler(root(receber_dados), receber_dados, []).  % Para receber cirurgia, staff, timetable, surgery_id, agenda_staff e agenda_operation_room



%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Converte o tempo no formato  "HH:MM:SS" para minutos
convert_to_minutes(TimeString, Minutes) :-
    split_string(TimeString, ":", "", [HStr, MStr, _]),
    number_string(Hours, HStr),
    number_string(MinutesPart, MStr),
    Minutes is Hours * 60 + MinutesPart.

% Converte string para átomo (usando valor padrão se a string for variável)
convert_to_atom(String, Atom) :-
    (var(String) -> Atom = 'unknown'; atom_string(Atom, String)).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Manipulador para a rota /receber_dados (recebe cirurgias, staff, timetable, surgery_id, agenda_staff e agenda_operation_room)
receber_dados(Pedido) :-
    cors_enable,
    http_read_json_dict(Pedido, DadosRecebidos),
    (   is_dict(DadosRecebidos)
    ->  processar_dados(DadosRecebidos),
        reply_json_dict(_{status: "sucesso", mensagem: "Dados armazenados com sucesso."})
    ;   reply_json_dict(_{status: "erro", mensagem: "JSON inválido."}, [status(400)])).

processar_dados(Dados) :-
    retract_all_fatos,
    maplist(chave_para_fato(Dados), [
        cirurgias-surgery,
        staff-staff,
        timetable-timetable,
        surgery_id-surgery_id,
        agenda_staff-agenda_staff,
        assignment_surgery-assignment_surgery,
        agenda_operation_room-agenda_operation_room
    ]).

chave_para_fato(Dados, Chave-Fato) :-
    (   get_dict(Chave, Dados, Lista), is_list(Lista)
    ->  processar_lista(Lista, Fato)
    ;   true).

processar_lista([], _).
processar_lista([Item|Rest], Fato) :-
    chamar_fato(Fato, Item),
    processar_lista(Rest, Fato).

chamar_fato(surgery, Item) :-
    convert_to_atom(Item.opTypeCode, OpTypeCode),
    convert_to_minutes(Item.anesthesia, Anesthesia),
    convert_to_minutes(Item.surgery, Surgery),
    convert_to_minutes(Item.cleaning, Cleaning),
    assertz(surgery(OpTypeCode, Anesthesia, Surgery, Cleaning)).

chamar_fato(staff, Item) :-
    convert_to_atom(Item.licenseNumber, LicenseNumber),
    convert_to_atom(Item.staffType, StaffType),
    convert_to_atom(Item.specialization, Specialization),
    maplist(convert_to_atom, Item.operationTypeCodes, OperationTypeCodes),
    assertz(staff(LicenseNumber, StaffType, Specialization, OperationTypeCodes)).

    chamar_fato(timetable, Item) :-
        convert_to_atom(Item.licenseNumber, LicenseNumber),
        number_string(Date, Item.date),  % Converte a string para número
        convert_to_minutes(Item.timeShiftEntrance, Entrance),
        convert_to_minutes(Item.timeShiftExit, Exit),
        assertz(timetable(LicenseNumber, Date, (Entrance, Exit))).
    
chamar_fato(surgery_id, Item) :-
    convert_to_atom(Item.opRequestCode, OpRequestCode),
    convert_to_atom(Item.opTypeCode, OpTypeCode),
    assertz(surgery_id(OpRequestCode, OpTypeCode)).

chamar_fato(agenda_staff, Item) :-
    convert_to_atom(Item.licenseNumber, LicenseNumber),
    number_string(Date, Item.date),  % Converte a string para número
    maplist(parse_agenda_schedule, Item.schedule, ParsedSchedule),
    assertz(agenda_staff(LicenseNumber, Date, ParsedSchedule)).

parse_agenda_schedule(ScheduleString, (Start, End, Code)) :-
    split_string(ScheduleString, "-", "", [StartStr, EndStr, CodeStr]),
    convert_to_minutes(StartStr, Start),
    convert_to_minutes(EndStr, End),
    convert_to_atom(CodeStr, Code).

chamar_fato(assignment_surgery, Item) :-
    convert_to_atom(Item.opRequestCode, OpRequestCode),
    convert_to_atom(Item.licenseNumber, LicenseNumber),
    assertz(assignment_surgery(OpRequestCode, LicenseNumber)).

chamar_fato(agenda_operation_room, Item) :-
    % Converte o número da sala
    convert_to_atom(Item.roomNumber, RoomNumber), % Converte o número da sala
    number_string(Date, Item.date),               % Converte a data para número
    maplist(parse_room_schedule_agenda, Item.schedule, ParsedSchedule), % Converte os horários
    assertz(agenda_operation_room(RoomNumber, Date, ParsedSchedule)).

% Função para parsear cada item de "schedule" (string no formato "startTime-endTime-opTypeCode")
parse_room_schedule_agenda(ScheduleString, (Start, End, OpTypeCode)) :-
    % Divide a string "startTime-endTime-opTypeCode" usando "-"
    split_string(ScheduleString, "-", "", [StartStr, EndStr, OpTypeCodeStr]), 

    % Converte as strings "startTime" e "endTime" para minutos desde a meia-noite
    convert_to_minutes(StartStr, Start),
    convert_to_minutes(EndStr, End).

    

parse_room_schedule(Schedule, (Start, End, OpTypeCode, Staff)) :-
    convert_to_atom(Schedule.opTypeCode, OpTypeCode),
    convert_to_minutes(Schedule.startTime, Start),
    convert_to_minutes(Schedule.endTime, End),
    maplist(convert_to_atom, Schedule.staff, Staff).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Consultas
consultar_cirurgias(_) :-
    findall(_{opTypeCode: OpTypeCode, anesthesia: Anesthesia, surgery: Surgery, cleaning: Cleaning},
            surgery(OpTypeCode, Anesthesia, Surgery, Cleaning), Resultado),
    reply_json_dict(Resultado).

consultar_staff(_) :-
    findall(_{licenseNumber: LicenseNumber, staffType: StaffType, specialization: Specialization, operationTypeCodes: OperationTypeCodes},
            staff(LicenseNumber, StaffType, Specialization, OperationTypeCodes), Resultado),
    reply_json_dict(Resultado).

consultar_timetable(_) :-
    findall(_{licenseNumber: LicenseNumber, date: Date, timeShift: (Entrance, Exit)},
            timetable(LicenseNumber, Date, (Entrance, Exit)), Resultado),
    reply_json_dict(Resultado).

consultar_surgery_id(_) :-
    findall(_{opRequestCode: OpRequestCode, opTypeCode: OpTypeCode},
            surgery_id(OpRequestCode, OpTypeCode), Resultado),
    reply_json_dict(Resultado).

consultar_agenda_staff(_) :-
    findall(_{licenseNumber: LicenseNumber, date: Date, schedule: Schedule},
            agenda_staff(LicenseNumber, Date, Schedule), Resultado),
    reply_json_dict(Resultado).

consultar_assignment_surgery(_) :-
    findall(_{opRequestCode: OpRequestCode, licenseNumber: LicenseNumber},
            assignment_surgery(OpRequestCode, LicenseNumber), Resultado),
    reply_json_dict(Resultado).

consultar_agenda_operation_room(_) :-
    findall(_{roomNumber: RoomNumber, date: Date, schedule: Schedule},
            agenda_operation_room(RoomNumber, Date, Schedule), Resultado),
    reply_json_dict(Resultado).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


% Código para obter e tratar as agendas livres
free_agenda0([],[(0,1440)]).
free_agenda0([(0,Tfin,_)|LT],LT1):-!,free_agenda1([(0,Tfin,_)|LT],LT1).
free_agenda0([(Tin,Tfin,_)|LT],[(0,T1)|LT1]):- T1 is Tin-1,
    free_agenda1([(Tin,Tfin,_)|LT],LT1).

free_agenda1([(_,Tfin,_)],[(T1,1440)]):-Tfin\==1440,!,T1 is Tfin+1.
free_agenda1([(_,_,_)],[]).
free_agenda1([(_,T,_),(T1,Tfin2,_)|LT],LT1):-Tx is T+1,T1==Tx,!,
    free_agenda1([(T1,Tfin2,_)|LT],LT1).
free_agenda1([(_,Tfin1,_),(Tin2,Tfin2,_)|LT],[(T1,T2)|LT1]):-T1 is Tfin1+1,T2 is Tin2-1,
    free_agenda1([(Tin2,Tfin2,_)|LT],LT1).

adapt_timetable(D,Date,LFA,LFA2):-timetable(D,Date,(InTime,FinTime)),treatin(InTime,LFA,LFA1),treatfin(FinTime,LFA1,LFA2).

treatin(InTime,[(In,Fin)|LFA],[(In,Fin)|LFA]):-InTime=<In,!.
treatin(InTime,[(_,Fin)|LFA],LFA1):-InTime>Fin,!,treatin(InTime,LFA,LFA1).
treatin(InTime,[(_,Fin)|LFA],[(InTime,Fin)|LFA]).
treatin(_,[],[]).

treatfin(FinTime,[(In,Fin)|LFA],[(In,Fin)|LFA1]):-FinTime>=Fin,!,treatfin(FinTime,LFA,LFA1).
treatfin(FinTime,[(In,_)|_],[]):-FinTime=<In,!.
treatfin(FinTime,[(In,_)|_],[(In,FinTime)]).
treatfin(_,[],[]).

% Intersecção das agendas para disponibilidade dos médicos e sala
intersect_all_agendas([Name],Date,LA):-!,availability(Name,Date,LA).
intersect_all_agendas([Name|LNames],Date,LI):-
    availability(Name,Date,LA),
    intersect_all_agendas(LNames,Date,LI1),
    intersect_2_agendas(LA,LI1,LI).

intersect_2_agendas([],_,[]).
intersect_2_agendas([D|LD],LA,LIT):-	intersect_availability(D,LA,LI,LA1),
					intersect_2_agendas(LD,LA1,LID),
					append(LI,LID,LIT).

intersect_availability((_,_),[],[],[]).

intersect_availability((_,Fim),[(Ini1,Fim1)|LD],[],[(Ini1,Fim1)|LD]):-
		Fim<Ini1,!.

intersect_availability((Ini,Fim),[(_,Fim1)|LD],LI,LA):-
		Ini>Fim1,!,
		intersect_availability((Ini,Fim),LD,LI,LA).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)],[(Fim,Fim1)|LD]):-
		Fim1>Fim,!,
		min_max(Ini,Ini1,_,Imax),
		min_max(Fim,Fim1,Fmin,_).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)|LI],LA):-
		Fim>=Fim1,!,
		min_max(Ini,Ini1,_,Imax),
		min_max(Fim,Fim1,Fmin,_),
		intersect_availability((Fim1,Fim),LD,LI,LA).


min_max(I,I1,I,I1):- I<I1,!.
min_max(I,I1,I1,I).

% Agendamento das cirurgias
schedule_all_surgeries(Room,Day):-
    retractall(agenda_staff1(_,_,_)),
    retractall(agenda_operation_room1(_,_,_)),
    retractall(availability(_,_,_)),
    findall(_,(agenda_staff(D,Day,Agenda),assertz(agenda_staff1(D,Day,Agenda))),_),
    agenda_operation_room(Or,Date,Agenda),assert(agenda_operation_room1(Or,Date,Agenda)),
    findall(_,(agenda_staff1(D,Date,L),free_agenda0(L,LFA),adapt_timetable(D,Date,LFA,LFA2),assertz(availability(D,Date,LFA2))),_),
    findall(OpCode,surgery_id(OpCode,_),LOpCode),
    availability_all_surgeries_with_all_staff(LOpCode,Room,Day),!.


% Verificação de disponibilidade e atualização das agendas
availability_all_surgeries_with_all_staff([], _, _).
availability_all_surgeries_with_all_staff([OpCode | LOpCode], Room, Day) :-
    (   
        % Tentar agendar a operação atual
        surgery_id(OpCode, OpType),
        surgery(OpType, TAnesthesia, TSurgery, TCleaning),
        TTotal is TAnesthesia + TSurgery + TCleaning,
        availability_operation(OpCode, Room, Day, LPossibilities, LDoctors),
        schedule_first_interval(TTotal, LPossibilities, (TinS, TfinS)),
        retract(agenda_operation_room1(Room, Day, Agenda)),
        insert_agenda((TinS, TfinS, OpCode), Agenda, Agenda1),
        assertz(agenda_operation_room1(Room, Day, Agenda1)),

        % Agendar anestesista
        obtain_staff_speciality(OpCode, anaesthetist, LAnesthesia),
        TinAnesthesia is TinS,
        TfinAnesthesia is TinS + TAnesthesia + TSurgery,
        insert_agenda_doctors((TinAnesthesia, TfinAnesthesia, OpCode), Day, LAnesthesia),

        % Agendar cirurgião
        obtain_staff_speciality(OpCode, orthopaedics, LSurgery),
        TinSurgery is TinS + TAnesthesia + 1,
        TfinSurgery is TinSurgery + TSurgery,
        insert_agenda_doctors((TinSurgery, TfinSurgery, OpCode), Day, LSurgery),

        % Agendar limpeza
        obtain_staff_speciality(OpCode, cleaning, LCleaning),
        TinCleaning is TfinSurgery + 1,
        TfinCleaning is TfinSurgery + TCleaning,
        insert_agenda_doctors((TinCleaning, TfinCleaning, OpCode), Day, LCleaning)
    ->  true
    ;   true
    ),
    % Continuar com as próximas operações
    availability_all_surgeries_with_all_staff(LOpCode, Room, Day).

        
        
        obtain_staff_speciality(OpCode, Specialty, LStaff) :-
            surgery_id(OpCode, SurgeryType),  % Obter tipo de cirurgia associado ao OpCode
            findall(StaffId, (
                staff(StaffId, _, Specialty, Specialties), % Buscar por membros da equipe com a especialidade desejada
                member(SurgeryType, Specialties)          % Verificar se a especialidade cobre o tipo da cirurgia
            ), LStaff).
% MODIFIED
availability_operation(OpCode, Room, Day, LPossibilities, LDoctors) :-
    surgery_id(OpCode, OpType),
    surgery(OpType, TAnesthesia, TSurgery, TCleaning),
    TotalTime is TAnesthesia + TSurgery + TCleaning,  % Tempo total (anestesia, cirurgia, limpeza)
    findall(Doctor, assignment_surgery(OpCode, Doctor), LDoctors),
    intersect_all_agendas(LDoctors, Day, LA),
    agenda_operation_room1(Room, Day, LAgenda),
    free_agenda0(LAgenda, LFAgRoom),
    intersect_2_agendas(LA, LFAgRoom, LIntAgDoctorsRoom),
    remove_unf_intervals(TotalTime, LIntAgDoctorsRoom, LPossibilities).  % Verifica disponibilidade para o tempo total

% MODIFIED
remove_unf_intervals(_, [], []).
remove_unf_intervals(TotalTime, [(Tin, Tfin) | LA], [(Tin, Tfin) | LA1]) :-
    DT is Tfin - Tin + 1,
    TotalTime =< DT,  % Verifica se o intervalo comporta o tempo total (anestesia + cirurgia + limpeza)
    !,
    remove_unf_intervals(TotalTime, LA, LA1).
remove_unf_intervals(TotalTime, [_ | LA], LA1) :-
    remove_unf_intervals(TotalTime, LA, LA1).


schedule_first_interval(TotalTime, [(Tin, _) | _], (Tin, TfinS)) :-
    TfinS is Tin + TotalTime - 1.  

insert_agenda((TinS,TfinS,OpCode),[],[(TinS,TfinS,OpCode)]).
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(TinS,TfinS,OpCode),(Tin,Tfin,OpCode1)|LA]):-TfinS<Tin,!.
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(Tin,Tfin,OpCode1)|LA1]):-insert_agenda((TinS,TfinS,OpCode),LA,LA1).

insert_agenda_doctors(_,_,[]).
insert_agenda_doctors((TinS,TfinS,OpCode),Day,[Doctor|LDoctors]):-
    retract(agenda_staff1(Doctor,Day,Agenda)),
    insert_agenda((TinS,TfinS,OpCode),Agenda,Agenda1),
    assert(agenda_staff1(Doctor,Day,Agenda1)),
    insert_agenda_doctors((TinS,TfinS,OpCode),Day,LDoctors).

%--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter,  TFinOp) :-
    get_time(Ti),
    (obtain_better_sol1(Room, Day) ; true),
    retract(better_sol(Day, Room, AgOpRoomBetter, LAgDoctorsBetter, TFinOp)),
    get_time(Tf),
    T is Tf - Ti.
    

% CONTINUA A OBTER DUPLICADOS MAS ENVOLVE TODA A STAFF
obtain_better_sol1(Room, Day) :-
    asserta(better_sol(Day, Room, _, _, 1441)),
    findall(OpCode, surgery_id(OpCode, _), LOC),
    permutation(LOC, LOpCode),
    
    retractall(agenda_staff1(_, _, _)),
    retractall(agenda_operation_room1(_, _, _)),
    retractall(availability(_, _, _)),
    findall(_, (agenda_staff(D, Day, Agenda), assertz(agenda_staff1(D, Day, Agenda))), _),
    agenda_operation_room(Room, Day, Agenda),
    assert(agenda_operation_room1(Room, Day, Agenda)),
    findall(_, (agenda_staff1(D, Day, L), free_agenda0(L, LFA), adapt_timetable(D, Day, LFA, LFA2), assertz(availability(D, Day, LFA2))), _),
    availability_all_surgeries_with_all_staff(LOpCode, Room, Day),
    agenda_operation_room1(Room, Day, AgendaR),
    update_better_sol(Day, Room, AgendaR, LOpCode),
    fail.

update_better_sol(Day, Room, Agenda, LOpCode) :-
    better_sol(Day, Room, _, _, FinTime),
    reverse(Agenda, AgendaR),
    evaluate_final_time(AgendaR, LOpCode, FinTime1),
    FinTime1 < FinTime,
    retract(better_sol(_, _, _, _, _)),
    % Obter agendas dos médicos
    findall(Doctor, assignment_surgery(_, Doctor), LDoctors1),
    remove_equals(LDoctors1, LDoctors),
    

    % Obter agendas de todo o staff
    generate_staff_agendas(Day, LDoctorAgendas, _),
   
    asserta(better_sol(Day, Room, Agenda,  LDoctorAgendas, FinTime1)).




evaluate_final_time([], _, 1441).
evaluate_final_time([(_, Tfin, _) | _], LOpCode, Tfin) :-
    member(_, LOpCode), !.  % Aqui, a variável OpCode não está sendo usada diretamente.
evaluate_final_time([(_, Tfin, _) | AgR], LOpCode, TfinFinal) :-
    evaluate_final_time(AgR, LOpCode, TfinTemp),
    TfinFinal is max(Tfin, TfinTemp).



% Função que gera a lista de agendas do staff
list_staff_agenda(_, [], []).
list_staff_agenda(Day, [D | LD], [(D, UniqueAg) | LAgD]) :-
agenda_staff1(D, Day, AgD),
remove_equals(AgD, UniqueAg),  % Remove duplicatas na agenda
list_staff_agenda(Day, LD, LAgD).

% Função para remover duplicatas de uma lista
remove_equals([], []).
remove_equals([X | L], L1) :-
member(X, L), !,
remove_equals(L, L1).
remove_equals([X | L], [X | L1]) :-
remove_equals(L, L1).

% Função para listar as agendas dos médicos
list_doctors_agenda(_, [], []).

list_doctors_agenda(Day, [D | LD], [(D, UniqueAg) | LAgD]) :-
agenda_staff1(D, Day, AgD),
remove_equals(AgD, UniqueAg),  % Remover duplicatas na agenda
list_doctors_agenda(Day, LD, LAgD).

% Função para listar as agendas dos enfermeiros
list_nurses_agenda(_, [], []).

list_nurses_agenda(Day, [N | LN], [(N, UniqueAg) | LAgN]) :-
agenda_staff1(N, Day, AgN),
remove_equals(AgN, UniqueAg),  % Remover duplicatas na agenda
list_nurses_agenda(Day, LN, LAgN).


% Função para listar as agendas de limpeza
list_cleaning_agenda(_, [], []).

list_cleaning_agenda(Day, [C | LC], [(C, UniqueAg) | LAgC]) :-
agenda_staff1(C, Day, AgC),
remove_equals(AgC, UniqueAg),  % Remover duplicatas na agenda
list_cleaning_agenda(Day, LC, LAgC).

% Função para gerar as agendas de limpeza
generate_staff_agendas(Day, LCleaningAgendas, IdRetrieve) :-
% Obter IDs de todos os membros do staff, mas apenas limpeza (ID começando com "t")
findall(StaffId, (
    agenda_staff1(StaffId, Day, _),
    sub_atom(StaffId, 0, 1, _, IdRetrieve)  % Filtra apenas os IDs que começam com "t"
), AllCleaning),
remove_equals(AllCleaning, UniqueCleaning),  % Remove duplicatas de limpeza
list_cleaning_agenda(Day, UniqueCleaning, LCleaningAgendas).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_json)).
:- use_module(library(http/http_parameters)).
:- use_module(library(lists)).
:- use_module(library(http/http_cors)).

:- set_prolog_flag(encoding, utf8).

% Cors: Permitir requisições de qualquer origem
:- set_setting(http:cors, [*]).

% Rota para calcular a melhor solução com obtain_better_sol/5
:- http_handler(root(calcular_better_sol_simple), handle_obtain_better_sol_simple, []).

% Predicado para processar a requisição de /calcular_better_sol_simple
handle_obtain_better_sol_simple(Request) :-
    % Ativa CORS para a requisição
    cors_enable,

    % Processa os parâmetros da requisição HTTP
    http_parameters(Request, [
        room(Room, [atom]),   % Recebe a sala como um átomo
        day(Day, [integer])   % Recebe o dia como um inteiro
    ]),

    % Chama o predicado obtain_better_sol/5 com os parâmetros recebidos
    (   obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, TFinOp)
    ->  % Converte listas complexas para JSON
        convert_segment_list_to_json(AgOpRoomBetter, JsonAgOpRoomBetter),
        convert_doctors_list_to_json(LAgDoctorsBetter, JsonLAgDoctorsBetter),

        % Prepara a resposta JSON
        reply_json_dict(_{
            status: "success",
            room: Room,
            day: Day,
            ag_op_room_better: JsonAgOpRoomBetter,
            ag_doctors_better: JsonLAgDoctorsBetter,  % Aqui incluímos os dados dos médicos
            final_time: TFinOp
        }, [encoding(utf8)])
    ;   % Caso obtain_better_sol/5 falhe
        reply_json_dict(_{
            status: "error",
            message: "Could not compute better solution for the provided inputs."
        }, [status(400)])
    ).


% Converte uma lista de segmentos (operações) para JSON
convert_segment_list_to_json([], []).
convert_segment_list_to_json([(Start, End, OpCode) | Tail], [Reply | JsonArray]) :-
    nonvar(Start), nonvar(End), nonvar(OpCode),  % Garante que os argumentos estão instanciados
    Reply = _{start: Start, end: End, operation: OpCode},
    convert_segment_list_to_json(Tail, JsonArray).
convert_segment_list_to_json([_ | Tail], JsonArray) :-
    % Ignora itens não instanciados e continua a conversão
    convert_segment_list_to_json(Tail, JsonArray).

% Converte uma lista de agendas do staff para JSON
convert_doctors_list_to_json([], []).
convert_doctors_list_to_json([(Staff, Agenda) | Tail], [Reply | JsonArray]) :-
    nonvar(Staff), nonvar(Agenda),  % Garante que os argumentos estão instanciados
    atom_string(Staff, StaffS),
    convert_segment_list_to_json(Agenda, JsonAgenda),
    Reply = _{staff: StaffS, agenda: JsonAgenda},
    convert_doctors_list_to_json(Tail, JsonArray).
convert_doctors_list_to_json([_ | Tail], JsonArray) :-
    % Ignora itens não instanciados e continua a conversão
    convert_doctors_list_to_json(Tail, JsonArray).


% Retract de todos os fatos dinâmicos
retract_all_fatos :-
    retractall(surgery(_, _, _, _)),
    retractall(staff(_, _, _, _)),
    retractall(timetable(_, _, _)),
    retractall(surgery_id(_, _)),
    retractall(agenda_staff(_, _, _)),
    retractall(assignment_surgery(_, _)),
    retractall(agenda_operation_room(_, _, _)).