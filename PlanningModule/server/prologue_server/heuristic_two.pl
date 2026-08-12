:- dynamic availability/3.
:- dynamic agenda_staff/3.
:- dynamic agenda_staff1/3.
:-dynamic agenda_operation_room/3.
:-dynamic agenda_operation_room1/3.
:-dynamic better_sol/5.

agenda_staff(d9767, 20241216, [(720, 790, m01), (1080, 1140, c01)]).
agenda_staff(d8290, 20241216, [(901, 960, m02), (1380, 1440, c02)]).
agenda_staff(d9368, 20241216, [(720, 790, m01), (910, 980, m02)]).
agenda_staff(d7282, 20241216, []).
agenda_staff(d0719, 20241216, []).

agenda_staff(n7073, 20241216, []).
agenda_staff(n5177, 20241216, []).
agenda_staff(t1756, 20241216, []).
agenda_staff(n7902, 20241216, []).
agenda_staff(n1238, 20241216, []).
agenda_staff(n2519, 20241216, []).
agenda_staff(t0534, 20241216, []).

agenda_staff(d9767, 20241215, []).
agenda_staff(d8290, 20241215, []).
agenda_staff(d9368, 20241215, []).
agenda_staff(d7282, 20241215, []).
agenda_staff(d0719, 20241215, []).

agenda_staff(n7073, 20241215, []).
agenda_staff(n5177, 20241215, []).
agenda_staff(t1756, 20241215, []).
agenda_staff(n7902, 20241215, []).
agenda_staff(n1238, 20241215, []).
agenda_staff(n2519, 20241215, []).
agenda_staff(t0534, 20241215, []).

timetable(d9767, 20241216, (480, 1200)).
timetable(d8290, 20241216, (500, 1440)).
timetable(d9368, 20241216, (520, 1320)).
timetable(d7282, 20241216, (480, 1200)).
timetable(d0719, 20241216, (500, 1440)).
timetable(n7073, 20241216, (520, 1320)).

timetable(n5177, 20241216, (500, 1440)).
timetable(t1756, 20241216, (520, 1320)).
timetable(n7902, 20241216, (480, 1200)).
timetable(n1238, 20241216, (500, 1440)).
timetable(n2519, 20241216, (520, 1320)).
timetable(t0534, 20241216, (520, 1320)).

timetable(d9767, 20241215, (480, 1200)).
timetable(d8290, 20241215, (500, 1440)).
timetable(d9368, 20241215, (520, 1320)).
timetable(d7282, 20241215, (480, 1200)).
timetable(d0719, 20241215, (500, 1440)).
timetable(n7073, 20241215, (520, 1320)).

timetable(n5177, 20241215, (500, 1440)).
timetable(t1756, 20241215, (520, 1320)).
timetable(n7902, 20241215, (480, 1200)).
timetable(n1238, 20241215, (500, 1440)).
timetable(n2519, 20241215, (520, 1320)).
timetable(t0534, 20241215, (520, 1320)).

staff(d9767, doctor, orthopedics, [ot0001, ot0002, ot0003]).
staff(d8290, doctor, orthopedics, [ot0001, ot0002, ot0003]).
staff(d9368, doctor, orthopedics, [ot0001, ot0002, ot0003]).
staff(d7282, doctor, anaesthetist, [ot0001, ot0002, ot0003]).
staff(d0719, doctor, anaesthetist, [ot0001, ot0002, ot0003]).

staff(n7073, nurse, instrumenting, [ot0001, ot0002, ot0003]).
staff(n5177, nurse, anaesthetist, [ot0001, ot0002, ot0003]).
staff(t1756, technician, cleaning, [ot0001, ot0002, ot0003]).
staff(n7902, nurse, instrumenting, [ot0001, ot0002, ot0003]).
staff(n1238, nurse, circulating, [ot0001, ot0002, ot0003]).
staff(n2519, nurse, anaesthetist, [ot0001, ot0002, ot0003]).
staff(t0534, technician, cleaning, [ot0001, ot0002, ot0003]).

%surgery(SurgeryType, TAnesthesia, TSurgery, TCleaning).
surgery(ot0001, 45, 60, 45).
surgery(ot0002, 45, 90, 45).
surgery(ot0003, 45, 75, 45).

surgery_id(or0001, ot0001).
surgery_id(or0002, ot0002).
surgery_id(or0003, ot0003).
surgery_id(or0004, ot0001).
surgery_id(or0005, ot0003).

assignment_surgery(or0001, d9767).
assignment_surgery(or0001, d7282).
assignment_surgery(or0002, d8290).
assignment_surgery(or0002, d0719).
assignment_surgery(or0003, d9368).
assignment_surgery(or0003, d7282).
assignment_surgery(or0004, d9767).
assignment_surgery(or0004, d0719).
assignment_surgery(or0005, d8290).
assignment_surgery(or0005, d9368).

agenda_operation_room(r0001,20241214,[]).
agenda_operation_room(r0002,20241214,[]).
agenda_operation_room(r0003,20241214,[]).

agenda_operation_room(r0001,20241215,[]).
agenda_operation_room(r0002,20241215,[]).
agenda_operation_room(r0003,20241215,[]).



% Agenda da sala.
agenda_operation_room(or1,20241028,[]).

%Bloco para calculcar tempo livre de uma agenda.
%free_agenda0/2([(i,f,_),...],R)
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
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Este bloco adapta a timetable do funcionario para corresponder
%ao horario disponivel.
%adapt_timetable/4(Staff,Data,ListaTemposLivres,R)
adapt_timetable(D,Date,LFA,LFA2):-timetable(D,Date,(InTime,FinTime)),treatin(InTime,LFA,LFA1),treatfin(FinTime,LFA1,LFA2).

treatin(InTime,[(In,Fin)|LFA],[(In,Fin)|LFA]):-InTime=<In,!.
treatin(InTime,[(_,Fin)|LFA],LFA1):-InTime>Fin,!,treatin(InTime,LFA,LFA1).
treatin(InTime,[(_,Fin)|LFA],[(InTime,Fin)|LFA]).
treatin(_,[],[]).

treatfin(FinTime,[(In,Fin)|LFA],[(In,Fin)|LFA1]):-FinTime>=Fin,!,treatfin(FinTime,LFA,LFA1).
treatfin(FinTime,[(In,_)|_],[]):-FinTime=<In,!.
treatfin(FinTime,[(In,_)|_],[(In,FinTime)]).
treatfin(_,[],[]).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


intersect_all_agendas([Name],Date,LA):-!,availability(Name,Date,LA).
intersect_all_agendas([Name|LNames],Date,LI):-
    availability(Name,Date,LA),
    intersect_all_agendas(LNames,Date,LI1),
    intersect_2_agendas(LA,LI1,LI).

intersect_2_agendas([],_,[]).
intersect_2_agendas([D|LD],LA,LIT):- intersect_availability(D,LA,LI,LA1),
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
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Marca a cirurgia no primeiro intervalo disponivel, sem considerar se existe
%tempo disponivel ou nao para concluir.
%schedule_first_interval/3(Cirurgia,Tempo disponivel,R)
schedule_first_interval(TSurgery,[(Tin,_)|_],(Tin,TfinS)):-
    TfinS is Tin + TSurgery - 1.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Insere um intervalo de tempo associado a uma cirurgia.
%Nao verifica sobreposiçao apenas insere na agenda.
%insert_agenda/3(Intervalo,Agenda,NovaAgenda)
insert_agenda((TinS,TfinS,OpCode),[],[(TinS,TfinS,OpCode)]).
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(TinS,TfinS,OpCode),(Tin,Tfin,OpCode1)|LA]):-TfinS<Tin,!.
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(Tin,Tfin,OpCode1)|LA1]):-insert_agenda((TinS,TfinS,OpCode),LA,LA1).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Insere os intervalos de tempo na agenda de cada medico selecionado.
%insert_agenda_doctors(Intervalo,Data,ListaMedicos)
insert_agenda_doctors(_,_,[]).
insert_agenda_doctors((TinS,TfinS,OpCode),Day,[Doctor|LDoctors]):-
    retract(agenda_staff1(Doctor,Day,Agenda)),
    insert_agenda((TinS,TfinS,OpCode),Agenda,Agenda1),
    assert(agenda_staff1(Doctor,Day,Agenda1)),
    insert_agenda_doctors((TinS,TfinS,OpCode),Day,LDoctors).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Bloco para verificar se é possivel marcar cirrugia em algum intervalo.
remove_unf_intervals(_,[],[]).
remove_unf_intervals(TSurgery,[(Tin,Tfin)|LA],[(Tin,Tfin)|LA1]):-DT is Tfin-Tin+1,TSurgery=<DT,!,
    remove_unf_intervals(TSurgery,LA,LA1).
remove_unf_intervals(TSurgery,[_|LA],LA1):- remove_unf_intervals(TSurgery,LA,LA1).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%Começo da euristica dois%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Bloco para calcular ocupaçao dos medicos
%%all_doctor_ocupancy/2(listamedicos,R)
% Predicado principal para somar os intervalos de tempo livre
calculate_free_time([], 0).  % Caso base: se a lista estiver vazia, o tempo livre é 0
calculate_free_time([(In, Fin)|LT], TotalFreeTime) :-
    % Para cada intervalo (In, Fin), calcula a diferença (Fin - In) e soma recursivamente
    FreeTime is Fin - In,
    calculate_free_time(LT, RemainingTime),
    TotalFreeTime is RemainingTime + FreeTime.

get_total_time_free(D,Date,TotalFreeTime):-
    agenda_staff(D,Date,L),
free_agenda0(L,LFA),
    adapt_timetable(D,Date,LFA,LFA2),
    calculate_free_time(LFA2,TotalFreeTime).

calculate_working_hours(D, Date, R) :-
    timetable(D, Date, (InTime, FinTime)),
    R is FinTime - InTime.

staff_occupation_percentage(D,Date,R):-
    get_total_time_free(D,Date,TFT),
    calculate_working_hours(D,Date,WH),
    R is (TFT/WH) * 100.

% Função principal para calcular a ocupação de uma lista de médicos.
all_doctor_occupation(Date,[(Doctor, OccupationPercent) | R]) :-
    findall((Doctor, OccupationPercent), (staff_occupation_percentage(Doctor, Date, OccupationPercent)), R).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Ordena os médicos pela ocupação em ordem decrescente
sort_doctors_by_occupation(DoctorOccupations, SortedDoctors) :-
    % Ordena a lista de médicos pela ocupação (em ordem decrescente)
    sort(2, @>=, DoctorOccupations, SortedDoctorsWithOccupation),
    % Extrai apenas os médicos da lista ordenada
    findall(Doctor, member((Doctor, _), SortedDoctorsWithOccupation), SortedDoctors).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Bloco para encontrar medico mais ocupado.
% Encontra o médico com maior ocupação a partir da lista de ocupações
find_most_occupied_doctor(DoctorOccupations, SortedDoctors) :-
    sort_doctors_by_occupation(DoctorOccupations, SortedDoctors).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Verifica se existem cirurgias para o medico selecionado e seleciona todas.
search_surgery_for_doctor([], _, []) :-
    !.  % Caso base: se a lista de médicos está vazia, não há cirurgias para marcar.
   
search_surgery_for_doctor(_, [], []) :-
    !.  % Caso base: se a lista de cirurgias pendentes está vazia, não há mais cirurgias para marcar.

search_surgery_for_doctor(LOpCode, [Doctor | Rest], OpCode) :-
    % Filtra as cirurgias da lista LOpCode que são associadas ao médico atual (Doctor)
    findall(OpCode, (member(OpCode, LOpCode), assignment_surgery(OpCode, Doctor)), LOpCodeForDoctor),
   
    % Se encontrar cirurgias, retorna a primeira OpCode encontrada
    (  
        LOpCodeForDoctor \= []
    ->  select(OpCode, LOpCodeForDoctor, _),  % Seleciona uma cirurgia da lista
        true  % Sai e utiliza a cirurgia encontrada
    ;  
        % Se não encontrou cirurgias, tenta o próximo médico
        search_surgery_for_doctor(LOpCode, Rest, OpCode)
    ).

select_surgery([OpCode | _], OpCode).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%Aqui faço a remoçao da cirurgia marcada/analisada da lista de cirurgias.
remove_surgery_from_list(_, [], []).  
remove_surgery_from_list(OpCode, [OpCode | Rest], Rest).
remove_surgery_from_list(OpCode, [Head | Rest], [Head | NewRest]) :-
    remove_surgery_from_list(OpCode, Rest, NewRest).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Predicado para tentar marcar todas as cirurgias de uma lista de cirurgias para os médicos
attempt_to_mark_surgeries([],_,_) :- !.
attempt_to_mark_surgeries(LOpCode,Room,Day) :-
    all_doctor_occupation(Day, DoctorOccupations),
    find_most_occupied_doctor(DoctorOccupations, SortedDoctors),
    search_surgery_for_doctor(LOpCode,SortedDoctors, OpCode),
    mark_surgery_if_possible(OpCode,Room,Day),
    remove_surgery_from_list(OpCode,LOpCode,NewLOpCode),
    attempt_to_mark_surgeries(NewLOpCode,Room,Day).

mark_surgery_if_possible(OpCode,Room,Day) :-
    surgery_id(OpCode,OpType),
    surgery(OpType,TAnaes,TSurgery,TClean),
    total_time(TAnaes,TSurgery,TATotal),
    total_time(TATotal,TClean,TTotal),
    obtain_staff_speciality(doctor, anaesthetist, LADoctors),
    all_staff_occupation(Day,LADoctors,Result1),
    sort_staff_by_occupation(Result1,SortedStaff1),
    obtain_staff_speciality(nurse, anaesthetist, LANurses),
    all_staff_occupation(Day,LANurses,Result2),
    sort_staff_by_occupation(Result2,SortedStaff2),
    obtain_staff_speciality(technician, cleaning, LMAAs),
    all_staff_occupation(Day,LMAAs,Result3),
    sort_staff_by_occupation(Result3,SortedStaff3),
    obtain_staff_speciality(nurse, instrumenting, LINurses),
    all_staff_occupation(Day,LINurses,Result4),
    sort_staff_by_occupation(Result4,SortedStaff4),
    obtain_staff_speciality(nurse, circulating, LCNurses),
    all_staff_occupation(Day,LCNurses,Result5),
    sort_staff_by_occupation(Result5,SortedStaff5),
    select_element(SortedStaff1,AD),
    select_element(SortedStaff2,AN),
    select_element(SortedStaff3,MAA),
    select_element(SortedStaff4,IN),
    select_element(SortedStaff5,CN),
    findall(Doctor, assignment_surgery(OpCode, Doctor), LDoctors),
    append([[AD],[AN], [IN], [CN], LDoctors, [MAA]], TotalStaff),
    intersect_all_agendas(TotalStaff, Day, ATotalStaff),
    agenda_operation_room1(Room,Day,LAgenda),
    free_agenda0(LAgenda,LFAgRoom),
    intersect_2_agendas(LFAgRoom,ATotalStaff,GlobalAgenda),
    remove_unf_intervals(TTotal,GlobalAgenda,LPossibilities),
    ( 
        LPossibilities \= [] -> 
        (
            schedule_first_interval(TTotal, LPossibilities, (TinS, TfinS)),
            TfinAnae is TinS + TATotal,
            TinDoc is TinS + TAnaes,
            TfinDoc is TinDoc + TSurgery,
            TinMAA is TinS + TATotal,

            retract(agenda_operation_room1(Room, Day, Agenda)),
            insert_agenda((TinS, TfinS, OpCode), Agenda, Agenda1),
            assertz(agenda_operation_room1(Room, Day, Agenda1)),
            insert_agenda_doctors((TinDoc, TfinDoc, OpCode), Day, LDoctors),
            insert_agenda_staff((TinS, TfinAnae, OpCode), Day, AD),
            insert_agenda_staff((TinS, TfinAnae, OpCode), Day, AN),
            insert_agenda_staff((TinMAA, TfinS, OpCode), Day, MAA),
            insert_agenda_staff((TinDoc, TfinDoc, OpCode), Day, IN),
            insert_agenda_staff((TinDoc, TfinDoc, OpCode), Day, CN)
        )
        ;
        true
    ).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%



% Caso base: lista vazia retorna resultado vazio.
all_staff_occupation(_, [], []).
% Caso recursivo: calcula a ocupação para o primeiro membro e continua com o restante.
all_staff_occupation(Date, [Staff | RestStaff], [(Staff, OccupationPercent) | RestResult]) :-
    staff_occupation_percentage(Staff, Date, OccupationPercent),
    all_staff_occupation(Date, RestStaff, RestResult).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Ordena os médicos pela ocupação em ordem decrescente
sort_staff_by_occupation(RestResult, SortedStaff) :-
    % Ordena a lista de médicos pela ocupação (em ordem decrescente)
    sort(2, @=<, RestResult, SortedStaffWithOccupation),
    % Extrai apenas os médicos da lista ordenada
    findall(Staff, member((Staff, _), SortedStaffWithOccupation), SortedStaff).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
obtain_staff_speciality(Type, Specialty, LStaffNoDuplicates) :-
    % Encontra todos os IDs que correspondem ao tipo e à especialidade.
    findall(StaffId, (
        staff(StaffId, Type, Specialty, _)
    ), LStaff),
    % Remove duplicatas da lista.
    remove_equals(LStaff, LStaffNoDuplicates).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

remove_equals([], []).
    remove_equals([H|T], R) :-
        member(H, T), !,       % Se H está no resto da lista, ignora H
        remove_equals(T, R).
    remove_equals([H|T], [H|R]) :-
        remove_equals(T, R).   % Caso contrário, mantém H
        
total_time(TAnaes,TSurgery,TTotal):-
    TTotal is TAnaes+TSurgery.

select_element([Element | _], Element).

insert_agenda_staff((TinS,TfinS,OpCode),Day,Staff):-
    retract(agenda_staff1(Staff,Day,Agenda)),
    insert_agenda((TinS,TfinS,OpCode),Agenda,Agenda1),
    assert(agenda_staff1(Staff,Day,Agenda1)).
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

schedule_surgeries_heuristics(Room,Day,LRoom,LDAgendas):-
    get_time(Ti),
    findall(OpCode, surgery_id(OpCode,_), LOC), !,
    % Obter todos os IDs de staff (não apenas médicos)
    findall(Staff, assignment_surgery(_,Staff), LStaff),
    retractall(agenda_staff1(_,_,_)),
    retractall(agenda_operation_room1(_,_,_)),
    retractall(availability(_,_,_)),
    findall(_, (agenda_staff(S,Day,Agenda), assertz(agenda_staff1(S,Day,Agenda))), _),
    agenda_operation_room(Room,Day,Agenda), assert(agenda_operation_room1(Room,Day,Agenda)),
    findall(_, (
        agenda_staff1(S,Day,L),
        free_agenda0(L,LFA),
        adapt_timetable(S,Day,LFA,LFA2),
        assertz(availability(S,Day,LFA2))
    ), _),
    findall(OpCode, surgery_id(OpCode,_), LOpCode),

    % A verdadeira heurística está aqui
    attempt_to_mark_surgeries(LOpCode,Room,Day), !,

    
    
        agenda_operation_room1(Room, Day, AgendaRoom),
     
   
    findall(S, agenda_staff1(S, Day, _), StaffList),
    
    % Atualizar resultados
    agenda_operation_room1(Room,Day,LRoom),
    
    collect_staff_agendas(Day,LDAgendas), !,
    get_time(Tf),
    T is Tf-Ti.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Gerar a lista completa de agendas para todos os membros do staff
list_staff_agenda(_, [], []).
list_staff_agenda(Day, [Staff | LStaff], [(Staff, UniqueAgStaff) | LAgStaff]) :-
    % Obter a agenda do membro específico de staff
    agenda_staff1(Staff, Day, AgStaff),
    remove_equals(AgStaff, UniqueAgStaff),  % Remover duplicatas na agenda
    list_staff_agenda(Day, LStaff, LAgStaff).

generate_staff_agendas(Day, StaffAgendas) :-
    % Obter IDs de todos os membros do staff (não apenas médicos)
    findall(StaffId, agenda_staff1(StaffId, Day, _), AllStaffIds),
    remove_equals(AllStaffIds, UniqueStaffIds),  % Garantir IDs únicos
    list_staff_agenda(Day, UniqueStaffIds, StaffAgendas).

collect_staff_agendas(Day, LDAgendas) :-
    % Encontra todos os IDs de staff com agenda para o dia
    findall(Staff, agenda_staff1(Staff, Day, _), AllStaff),
    remove_equals(AllStaff, UniqueStaff),  % Remove duplicatas
    % Para cada staff, obtém sua agenda e monta a lista
    findall((Staff, Agenda),
        (member(Staff, UniqueStaff), agenda_staff1(Staff, Day, Agenda)),
        LDAgendas
    ).
    
    
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_json)).
:- use_module(library(http/http_parameters)).
:- use_module(library(lists)).
:- use_module(library(http/http_cors)).

:- set_prolog_flag(encoding, utf8).


% Inicia o servidor na porta especificada
iniciar_servidor(Porta) :-
    http_server(http_dispatch, [port(Porta)]).

% Inicialização automática do servidor
:- initialization(iniciar_servidor(8082)).



% Cors: Permitir requisições de qualquer origem
:- set_setting(http:cors, [*]).

% Rota para calcular a melhor solução com obtain_better_sol/5
:- http_handler(root(calcular_heuristics_two), handle_obtain_better_sol_heuristic_two, []).

% Predicado para processar a requisição de /calcular_heuristics_two
handle_obtain_better_sol_heuristic_two(Request) :-
    % Ativa CORS para a requisição
    cors_enable,

    % Processa os parâmetros da requisição HTTP
        http_parameters(Request, [
            room(Room, [atom]),   % Recebe a sala como um átomo
            day(Day, [integer])   % Recebe o dia como um inteiro
        ]),
        
        
    % Chama o predicado obtain_better_sol/5 com os parâmetros recebidos
    (   schedule_surgeries_heuristics(Room, Day, AgOpRoomBetter, LAgDoctorsBetter)
    ->  % Converte listas complexas para JSON
        convert_segment_list_to_json(AgOpRoomBetter, JsonAgOpRoomBetter),
        convert_doctors_list_to_json(LAgDoctorsBetter, JsonLAgDoctorsBetter),

        % Prepara a resposta JSON
        reply_json_dict(_{
            status: "success",
            room: Room,
            day: Day,
            ag_op_room_better: JsonAgOpRoomBetter,
            ag_doctors_better: JsonLAgDoctorsBetter  % Aqui incluímos os dados dos médicos
            },[encoding(utf8)])
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
