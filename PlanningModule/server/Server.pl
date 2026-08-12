:- dynamic availability/3.
:- dynamic agenda_staff/3.
:- dynamic agenda_staff1/3.
:- dynamic agenda_operation_room/3.
:- dynamic agenda_operation_room1/3.
:- dynamic better_sol/5.

agenda_staff(d3704,20241128,[(720,790,m01),(1080,1140,c01)]).
agenda_staff(d8224,20241128,[(850,900,m02),(901,960,m02),(1380,1440,c02)]).
agenda_staff(d8163,20241128,[(720,790,m01),(910,980,m02)]).

timetable(d3704,20241128,(480,1200)).
timetable(d8224,20241128,(500,1440)).
timetable(d8163,20241128,(520,1320)).

% definindo os profissionais necessários para cada tipo de cirurgia
staff_required(so2, [1, 3, 1, 1, 1, 1]).  % 1 ortopedista, 3 ortopedistas, 1 anestesista, etc.
staff_required(so3, [1, 3, 1, 1, 1, 1]).  % 1 ortopedista, 3 ortopedistas, 1 anestesista, etc.
staff_required(so4, [1, 2, 1, 1, 1, 1]).  % 1 ortopedista, 2 ortopedistas, 1 anestesista, etc.

% definições de médicos e cirurgias com tempos de anestesia, cirurgia e limpeza
staff(d3704,doctor,orthopaedist,[ot0001,ot0002,ot0003]).
staff(d8224,doctor,orthopaedist,[ot0001,ot0002,ot0003]).
staff(d8163,doctor,orthopaedist,[ot0001,ot0002,ot0003]).

staff(n001,nurse,instrumenting,[so2,so3,so4]).
staff(n002,nurse,circulating,[so2,so3,so4]).
staff(n003,nurse,anaesthetist,[so2,so3,so4]).
staff(n004,nurse,medical_assistant,[so2,so3,so4]).

staff(a001,technician,anaesthetist,[so2,so3,so4]).
staff(a002,technician,anaesthetist,[so2,so3,so4]).

% surgery(surgerytype,tanesthesia,tsurgery,tcleaning).
surgery(ot0001,45,60,45).  % 45min de anestesia, 60min de cirurgia, 45min de limpeza
surgery(ot0002,45,90,45).  % 45min de anestesia, 90min de cirurgia, 45min de limpeza
surgery(ot0003,45,75,45).  % 45min de anestesia, 75min de cirurgia, 45min de limpeza

% associação dos ids de cirurgia ao tipo
surgery_id(or0001,ot0001).
surgery_id(or0006,ot0002).
surgery_id(or0002,ot0003).
surgery_id(or0004,ot0001).

% atribuição de prioridades às cirurgias
surgery_priority(or0001,1).  % alta prioridade
surgery_priority(or0002,3).  % baixa prioridade
surgery_priority(or0004,2).  % prioridade média
surgery_priority(or0006,1).  % alta prioridade

% atribuição de cirurgia a médicos específicos
assignment_surgery(or0001,d8224).
assignment_surgery(or0002,d3704).
assignment_surgery(or0004,d8163).
assignment_surgery(or0006,d8224).



% Agenda inicial da sala de operações
agenda_operation_room(r0001,20241128,[(600,670,or0003),(1100,1200,or0007)]).

% Código para obter e tratar as agendas livres
free_agenda0([],[(0,1440)]).
free_agenda0([(0,Tfin,_)|LT],LT1) :- !, free_agenda1([(0,Tfin,_)|LT],LT1).
free_agenda0([(Tin,Tfin,_)|LT],[(0,T1)|LT1]) :-
    T1 is Tin - 1,
    free_agenda1([(Tin,Tfin,_)|LT],LT1).

free_agenda1([(_,Tfin,_)],[(T1,1440)]) :- Tfin \== 1440, !, T1 is Tfin + 1.
free_agenda1([(_,_,_)],[]).
free_agenda1([(_,T,_),(T1,Tfin2,_)|LT],LT1) :- Tx is T + 1, T1 == Tx, !,
    free_agenda1([(T1,Tfin2,_)|LT],LT1).
free_agenda1([(_,Tfin1,_),(Tin2,Tfin2,_)|LT],[(T1,T2)|LT1]) :-
    T1 is Tfin1 + 1, T2 is Tin2 - 1,
    free_agenda1([(Tin2,Tfin2,_)|LT],LT1).

adapt_timetable(D,Date,LFA,LFA2) :-
    timetable(D,Date,(InTime,FinTime)),
    treatin(InTime,LFA,LFA1),
    treatfin(FinTime,LFA1,LFA2).

treatin(InTime,[(In,Fin)|LFA],[(In,Fin)|LFA]) :- InTime =< In, !.
treatin(InTime,[(_,Fin)|LFA],LFA1) :- InTime > Fin, !, treatin(InTime,LFA,LFA1).
treatin(InTime,[(_,Fin)|LFA],[(InTime,Fin)|LFA]).
treatin(_,[],[]).

treatfin(FinTime,[(In,Fin)|LFA],[(In,Fin)|LFA1]) :- FinTime >= Fin, !, treatfin(FinTime,LFA,LFA1).
treatfin(FinTime,[(In,_)|_],[]):- FinTime =< In, !.
treatfin(FinTime,[(In,_)|_],[(In,FinTime)]).
treatfin(_,[],[]).

% Intersecção das agendas para disponibilidade dos médicos e sala
intersect_all_agendas([Name],Date,LA) :- !, availability(Name,Date,LA).
intersect_all_agendas([Name|LNames],Date,LI) :-
    availability(Name,Date,LA),
    intersect_all_agendas(LNames,Date,LI1),
    intersect_2_agendas(LA,LI1,LI).

intersect_2_agendas([],_,[]).
intersect_2_agendas([D|LD],LA,LIT) :-
    intersect_availability(D,LA,LI,LA1),
    intersect_2_agendas(LD,LA1,LID),
    append(LI,LID,LIT).

intersect_availability((_,_),[],[],[]).
intersect_availability((_,Fim),[(Ini1,Fim1)|LD],[],[(Ini1,Fim1)|LD]) :- Fim < Ini1, !.
intersect_availability((Ini,Fim),[(_,Fim1)|LD],LI,LA) :- Ini > Fim1, !,
    intersect_availability((Ini,Fim),LD,LI,LA).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)],[(Fim,Fim1)|LD]) :-
    Fim1 > Fim, !,
    min_max(Ini,Ini1,_,Imax),
    min_max(Fim,Fim1,Fmin,_).

intersect_availability((Ini,Fim),[(Ini1,Fim1)|LD],[(Imax,Fmin)|LI],LA) :-
    Fim >= Fim1, !,
    min_max(Ini,Ini1,_,Imax),
    min_max(Fim,Fim1,Fmin,_),
    intersect_availability((Fim1,Fim),LD,LI,LA).

min_max(I,I1,I,I1) :- I < I1, !.
min_max(I,I1,I1,I).

% Agendamento das cirurgias
schedule_all_surgeries(Room,Day) :-
    retractall(agenda_staff1(_,_,_)),
    retractall(agenda_operation_room1(_,_,_)),
    retractall(availability(_,_,_)),
    findall(_, (agenda_staff(D,Day,Agenda), assertz(agenda_staff1(D,Day,Agenda))), _),
    agenda_operation_room(Room,Date,Agenda), assert(agenda_operation_room1(Room,Date,Agenda)),
    findall(_, (agenda_staff1(D,Date,L), free_agenda0(L,LFA), adapt_timetable(D,Date,LFA,LFA2), assertz(availability(D,Date,LFA2))), _),
    findall(OpCode, surgery_id(OpCode, _), LOpCode),
    sort_cirurgies_by_priority(LOpCode, SortedLOpCode),  % Ordena pelas prioridades
    availability_all_surgeries(SortedLOpCode, Room, Day).

% Ordenação por prioridade das cirurgias
sort_cirurgies_by_priority(_, SortedOpCodes) :-
    findall((Priority, OpCode), surgery_priority(OpCode, Priority), PrioritizedOpCodes),
    sort(PrioritizedOpCodes, SortedPrioritizedOpCodes),
    findall(OpCode, member((_, OpCode), SortedPrioritizedOpCodes), SortedOpCodes).

get_priority(OpCode, Priority-OpCode) :-
    surgery_priority(OpCode, Priority).

% Verificação de disponibilidade e atualização das agendas
availability_all_surgeries([], _, _).
availability_all_surgeries([OpCode|LOpCode], Room, Day) :-
    surgery_id(OpCode, OpType),
    surgery(OpType, TAnesthesia, TSurgery, TCleaning),
    TotalTime is TAnesthesia + TSurgery + TCleaning,  % Calcula o tempo total
    availability_operation(OpCode, Room, Day, LPossibilities, LDoctors),
    schedule_first_interval(TotalTime, LPossibilities, (TinS, TfinS)),  % Agendando com o tempo total
    retract(agenda_operation_room1(Room, Day, Agenda)),
    insert_agenda((TinS, TfinS, OpCode), Agenda, Agenda1),
    assertz(agenda_operation_room1(Room, Day, Agenda1)),
    insert_agenda_doctors((TinS, TfinS, OpCode), Day, LDoctors),
    availability_all_surgeries(LOpCode, Room, Day).
    
availability_operation(OpCode,Room,Day,LPossibilities,LDoctors):-
    surgery_id(OpCode,OpType),surgery(OpType,_,TSurgery,_),
    findall(Doctor,assignment_surgery(OpCode,Doctor),LDoctors),
    intersect_all_agendas(LDoctors,Day,LA),
    agenda_operation_room1(Room,Day,LAgenda),
    free_agenda0(LAgenda,LFAgRoom),
    intersect_2_agendas(LA,LFAgRoom,LIntAgDoctorsRoom),
    remove_unf_intervals(TSurgery,LIntAgDoctorsRoom,LPossibilities).



remove_unf_intervals(_,[],[]).
remove_unf_intervals(TSurgery,[(Tin,Tfin)|LA],[(Tin,Tfin)|LA1]):-DT is Tfin-Tin+1,TSurgery=<DT,!,
    remove_unf_intervals(TSurgery,LA,LA1).
remove_unf_intervals(TSurgery,[_|LA],LA1):- remove_unf_intervals(TSurgery,LA,LA1).    

schedule_first_interval(TotalTime, [(Tin, _) | _], (Tin, TfinS)) :-
    TfinS is Tin + TotalTime - 1.  % A duração total da cirurgia é a soma de anestesia, cirurgia e limpeza

insert_agenda((TinS,TfinS,OpCode),[],[(TinS,TfinS,OpCode)]).
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(TinS,TfinS,OpCode),(Tin,Tfin,OpCode1)|LA]):-TfinS<Tin,!.
insert_agenda((TinS,TfinS,OpCode),[(Tin,Tfin,OpCode1)|LA],[(Tin,Tfin,OpCode1)|LA1]):-insert_agenda((TinS,TfinS,OpCode),LA,LA1).

insert_agenda_doctors(_,_,[]).
insert_agenda_doctors((TinS,TfinS,OpCode),Day,[Doctor|LDoctors]):-
    retract(agenda_staff1(Doctor,Day,Agenda)),
    insert_agenda((TinS,TfinS,OpCode),Agenda,Agenda1),
    assert(agenda_staff1(Doctor,Day,Agenda1)),
    insert_agenda_doctors((TinS,TfinS,OpCode),Day,LDoctors).



    obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, TFinOp) :-
        get_time(Ti),
        (obtain_better_sol1(Room, Day); true),
        retract(better_sol(Day, Room, AgOpRoomBetter, LAgDoctorsBetter, TFinOp)),
       % write('Final Result: AgOpRoomBetter='), write(AgOpRoomBetter), nl,
       % write('LAgDoctorsBetter='), write(LAgDoctorsBetter), nl,
       % write('TFinOp='), write(TFinOp), nl,
        get_time(Tf),
        T is Tf - Ti.
      % write('Tempo de geracao da solucao:'), write(T), nl.
    
    % Função que gera a melhor solução considerando as prioridades
    obtain_better_sol1(Room, Day) :-
        asserta(better_sol(Day, Room, _, _, 1441)),
        findall(OpCode, surgery_id(OpCode, _), LOC),  % Lista todas as cirurgias
        sort_cirurgies_by_priority(LOC, LOpCode),  % Ordena as cirurgias pela prioridade
        permutation(LOpCode, LOpCodePerm),  % Permutação das cirurgias de acordo com a prioridade
        retractall(agenda_staff1(_, _, _)),
        retractall(agenda_operation_room1(_, _, _)),
        retractall(availability(_, _, _)),
        findall(_, (agenda_staff(D, Day, Agenda), assertz(agenda_staff1(D, Day, Agenda))), _),
        agenda_operation_room(Room, Day, Agenda),
        assert(agenda_operation_room1(Room, Day, Agenda)),
        findall(_, (agenda_staff1(D, Day, L), free_agenda0(L, LFA), adapt_timetable(D, Day, LFA, LFA2), assertz(availability(D, Day, LFA2))), _),
        availability_all_surgeries(LOpCodePerm, Room, Day),
        agenda_operation_room1(Room, Day, AgendaR),
        update_better_sol(Day, Room, AgendaR, LOpCodePerm),
        fail.
    
    % Função que avalia o tempo final considerando as prioridades
    evaluate_final_time([], _, 1441).
evaluate_final_time([(_, Tfin, _) | _], LOpCode, Tfin) :- 
    member(_, LOpCode), !.  % Aqui, a variável `OpCode` não está sendo usada diretamente.
evaluate_final_time([(_, Tfin, _) | AgR], LOpCode, TfinFinal) :-
    evaluate_final_time(AgR, LOpCode, TfinTemp),
    TfinFinal is max(Tfin, TfinTemp).
    
    % Função que atualiza a melhor solução, agora levando em consideração a prioridade das cirurgias
    update_better_sol(Day, Room, Agenda, LOpCode) :-
        better_sol(Day, Room, _, _, FinTime),
        reverse(Agenda, AgendaR),
        evaluate_final_time(AgendaR, LOpCode, FinTime1),
       % write('Analisando para LOpCode='), write(LOpCode), nl,
       % write('Agora: FinTime1='), write(FinTime1), write(' Agenda='), write(Agenda), nl,
        FinTime1 < FinTime,  % Se o tempo final for menor, atualiza
       % write('Melhor solution atualizada'), nl,
        retract(better_sol(_, _, _, _, _)),
        findall(Doctor, assignment_surgery(_, Doctor), LDoctors1),
        remove_equals(LDoctors1, LDoctors),
        list_doctors_agenda(Day, LDoctors, LDAgendas),
        asserta(better_sol(Day, Room, Agenda, LDAgendas, FinTime1)).
    
    % Função que gera a lista de médicos associados à agenda
    list_doctors_agenda(_, [], []).
    list_doctors_agenda(Day, [D | LD], [(D, AgD) | LAgD]) :-
        agenda_staff1(D, Day, AgD),
        list_doctors_agenda(Day, LD, LAgD).
    
    % Função que remove duplicatas de uma lista
    remove_equals([], []).
    remove_equals([X | L], L1) :-
        member(X, L), !,
        remove_equals(L, L1).
    remove_equals([X | L], [X | L1]) :-
        remove_equals(L, L1).



    :- use_module(library(http/thread_httpd)).
    :- use_module(library(http/http_dispatch)).
    :- use_module(library(http/http_json)).
    :- use_module(library(http/http_parameters)).
    :- use_module(library(lists)).
    :- use_module(library(http/http_cors)).
    
    :- set_prolog_flag(encoding, utf8).
    
    % Cors: Permitir requisições de qualquer origem
    :- set_setting(http:cors, [*]).
    
    % Rota para /obtain_better_solution
    :- http_handler(root(obtain_better_solution), handle_obtain_better_sol, []).
    
    % Rota para outras requisições, se necessário
    :- http_handler(root(sort_surgeries), handle_sort_surgeries, [method(get)]).
    
    % Servidor HTTP na porta 8080
    server(Port) :-
        http_server(http_dispatch, [port(Port)]).
    
    % Inicia o servidor automaticamente
    :- initialization(start_server).
    
    start_server :-
        server(8080),
        writeln('Servidor HTTP rodando na porta 8080').
    
    %--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    % Predicado para processar a requisição de /obtain_better_solution
    handle_obtain_better_sol(Request) :-
        % Ativa CORS para a requisição
        cors_enable,
    
        % Processa os parâmetros da requisição HTTP
        http_parameters(Request, [
            room(Room, [atom]),
            day(Day, [integer])
        ]),
    
        % Registra o tempo de início
        get_time(Ti),
    
        % Passa os parâmetros para a solução
        obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, TFinOp),
    
        % Registra o tempo de término
        get_time(Tf),
    
        % Calcula o tempo de execução
        T is Tf - Ti,
    
        % Formata os resultados
        maplist(format_ag_op_room_better, AgOpRoomBetter, AgOpRoomBetterFormatted),
        maplist(format_doctors_agenda, LAgDoctorsBetter, LAgDoctorsBetterFormatted),
    
        % Responde com JSON
        reply_json_dict(_{
            status: "success",
            ag_op_room_better: AgOpRoomBetterFormatted,
            doctors_agenda_better: LAgDoctorsBetterFormatted,
            final_time: TFinOp,
            generation_time: T  % Tempo de geração da solução
        }, [encoding(utf8)]).
    
    % Função auxiliar para formatar AgOpRoomBetter
    format_ag_op_room_better((Start, End, SurgeryID), [Start, End, SurgeryID]).
    
    % Função auxiliar para formatar LAgDoctorsBetter
    format_doctors_agenda((DoctorID, Schedule), _{doctor: DoctorID, schedule: FormattedSchedule}) :-
        % Formatar cada agenda do médico
        maplist(format_schedule_entry, Schedule, FormattedSchedule).
    
    % Função auxiliar para formatar cada item da agenda
    format_schedule_entry((Start, End, SurgeryID), _{start_time: Start, end_time: End, surgery_id: SurgeryID}).
    
    %----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------