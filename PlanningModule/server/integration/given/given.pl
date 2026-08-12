%% obtain_better_sol modificado.
%% update_better_sol modificado.

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
        obtain_staff_speciality(OpCode, orthopedics, LSurgery),
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
obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, LAgNursesBetter, LCleaning, TFinOp) :-
    get_time(Ti),
    (obtain_better_sol1(Room, Day) ; true),
    retract(better_sol(Day, Room, AgOpRoomBetter, LAgDoctorsBetter, LAgNursesBetter, LCleaning, TFinOp)),
    
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
    generate_staff_agendas(Day, LDoctorAgendas, 'd'),
    generate_staff_agendas(Day, LNurseAgendas, 'n'),
    generate_staff_agendas(Day, LCleaningAgendas, 't'),

asserta(better_sol(Day, Room, Agenda, LDoctorAgendas, LNurseAgendas , LCleaningAgendas, FinTime1)).


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

