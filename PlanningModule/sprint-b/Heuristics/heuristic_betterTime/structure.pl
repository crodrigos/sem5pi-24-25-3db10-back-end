% Verifica o tempo total necessário para realizar a cirurgia (usando o ID da operação)
get_surgery_time(OpID, TotalTime) :- 
    surgery_id(OpID, OpCode),  % Mapeia o ID para o tipo de cirurgia
    surgery(OpCode, TAnesthesia, TSurgery, TCleaning),
    TotalTime is TAnesthesia + TSurgery + TCleaning,
    write('Tempo total para a cirurgia (ID '), write(OpID), write('): '), write(TotalTime), nl.

% Converte o tempo em minutos para o formato de horas e minutos
minutes_to_time_format(Minutes, Hour, Minute) :-
    Hour is div(Minutes, 60),  % Converte minutos em horas
    Minute is mod(Minutes, 60).  % Converte minutos restantes para minutos

% Verifica se o médico está disponível para realizar a cirurgia e retorna o primeiro horário disponível
can_schedule_surgery(Doctor, Day, OpID) :- 
    write('Verificando se o médico pode agendar a cirurgia (ID '), write(OpID), write('): '), write(Doctor), nl,
    
    get_surgery_time(OpID, TotalTime),  % Calcula o tempo da cirurgia
    timetable(Doctor, Day, (WorkStart, WorkEnd)),  % Obtém o horário de trabalho do médico
    write('Horário de trabalho do médico: '), write((WorkStart, WorkEnd)), nl,
    
    agenda_staff(Doctor, Day, Agenda),  % Obtém a agenda do médico
    write('Agenda do médico: '), write(Agenda), nl,
    
    % Calcula o primeiro horário disponível na agenda do médico para essa cirurgia
    find_earliest_start_time_for_surgery(Agenda, WorkStart, WorkEnd, TotalTime, StartTime),
    
    % Converte o horário de início para o formato de horas e minutos
    minutes_to_time_format(StartTime, Hour, Minute),
    
    write('Médico '), write(Doctor), write(' pode realizar a cirurgia no horário: '),
    write(Hour), write(':'), write(Minute), nl.

% Encontra o primeiro horário disponível na agenda do médico para a cirurgia
find_earliest_start_time_for_surgery([], _, _, _, _) :- 
    fail.  % Caso não haja horário disponível

find_earliest_start_time_for_surgery([(Start, End, _) | Rest], WorkStart, WorkEnd, TotalTime, StartTime) :-
    % Verifica se o médico tem tempo disponível para realizar a cirurgia
    (   Start >= WorkStart,  % O horário de início da cirurgia não pode ser antes do início do expediente
        End + TotalTime =< WorkEnd  % O horário de término da cirurgia não pode ultrapassar o expediente
    ->  StartTime = End  % O horário de início será o término da cirurgia anterior
    ;   % Caso contrário, continua verificando o próximo horário
        find_earliest_start_time_for_surgery(Rest, WorkStart, WorkEnd, TotalTime, StartTime)
    ).

% Encontra o médico disponível mais cedo para realizar a cirurgia
find_earliest_available_doctor(OpID, Day, BestDoctor) :-
    write('Procurando médicos disponíveis para a cirurgia (ID '), write(OpID), write(')...'), nl,
    
    % Encontrar todos os médicos disponíveis para a cirurgia
    findall(Doctor, can_schedule_surgery(Doctor, Day, OpID), AvailableDoctors),
    write('Médicos disponíveis encontrados: '), write(AvailableDoctors), nl,
    
    % Se houver médicos disponíveis, escolhe o que tem o primeiro horário disponível
    (   AvailableDoctors \= []
    ->  find_earliest_start_time(AvailableDoctors, Day, OpID, BestDoctor)
    ;   BestDoctor = 'Nenhum disponível',
        write('Nenhum médico disponível para o horário solicitado.'), nl
    ).

% Função recursiva para encontrar o médico com o horário de início mais cedo
find_earliest_start_time([Doctor], Day, OpID, BestDoctor) :-
    write('Médico único encontrado, retornando: '), write(Doctor), nl,
    BestDoctor = Doctor.

find_earliest_start_time([Doctor1, Doctor2 | Rest], Day, OpID, BestDoctor) :-
    write('Comparando médicos para encontrar o horário mais cedo...'), nl,
    
    % Verifica o horário mais cedo para o primeiro médico
    can_schedule_surgery(Doctor1, Day, OpID),
    
    % Verifica o horário mais cedo para o segundo médico
    can_schedule_surgery(Doctor2, Day, OpID),
    
    % Compara os dois e escolhe o que pode começar mais cedo
    compare_start_times(Doctor1, Doctor2, BestDoctor).

% Compara os horários de início e escolhe o médico com o horário mais cedo
compare_start_times(Doctor1, Doctor2, BestDoctor) :-
    % Aqui você pode comparar os horários de disponibilidade de cada médico
    % para ver qual pode começar mais cedo. Como exemplo, vamos apenas escolher o primeiro.
    BestDoctor = Doctor1.  % Modifique conforme necessário para comparar realmente os horários

% Função que agenda as cirurgias para os médicos disponíveis
schedule_surgeries_heuristics(Room, Day, Heuristic, LRoom, LDAgendas) :-
    get_time(Ti),
    findall(OpCode, surgery_id(OpCode,_), LOC), !,
    findall(Doctor, assignment_surgery(_, Doctor), LDoctors),
    retractall(agenda_staff1(,,_)),
    retractall(agenda_operation_room1(,,_)),
    retractall(availability(,,_)),
    
    findall((agenda_staff(D, Day, Agenda), assertz(agenda_staff1(D, Day, Agenda))),),
    agenda_operation_room(Room, Day, Agenda), assert(agenda_operation_room1(Room, Day, Agenda)),
    
    findall((agenda_staff1(D, Day, L), free_agenda0(L, LFA), adapt_timetable(D, Day, LFA, LFA2), assertz(availability(D, Day, LFA2))),), 

    % Aloca as cirurgias
    schedule_surgeries_helper(LOC, Room, Day, Heuristic, [], _),

    agenda_operation_room1(Room, Day, LRoom),
    list_doctors_agenda(Day, LDoctors, LDAgendas), !,
    
    get_time(Tf),
    T is Tf - Ti,
    nl, write('Tempo de geração da solução:'), write(T), nl.
    
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

assign_doctors_to_surgeries(LOpCodeDoctors) :-
    findall(OpCode, surgery_id(OpCode, _), LOpCode),  % Obtém todos os códigos das cirurgias
    assign_doctors(LOpCode, LOpCodeDoctors).

% Predicado auxiliar para associar médicos a cada cirurgia
assign_doctors([], []).

assign_doctors([OpCode | LOpCode], [(OpCode, Doctors) | LOpCodeDoctors]) :-
    
    findall(Doctor, assignment_surgery(OpCode, Doctor), Doctors),
    
    assign_doctors(LOpCode, LOpCodeDoctors).
    
    
    schedule_surgeries_heuristics(Room,Day,LRoom,LDAgenda):-
    	get_time(Ti),
            retractall(agenda_staff1(_,_,_)),
            retractall(agenda_operation_room1(_,_,_)),
            retractall(availability(_,_,_)),
            findall(_,(agenda_staff(D,Day,Agenda),assertz(agenda_staff1(D,Day,Agenda))),_),
            agenda_operation_room(Or,Date,Agenda),assert(agenda_operation_room1(Or,Date,Agenda)),
            findall(_,(agenda_staff1(D,Date,L),free_agenda0(L,LFA),adapt_timetable(D,Date,LFA,LFA2),assertz(availability(D,Date,LFA2))),_),
            findall(OpCode,surgery_id(OpCode,_),LOpCode),
            assign_doctors_to_surgeries(LRoom).
            
            
           % Verifica o tempo total necessário para realizar a cirurgia (usando o ID da operação)
           get_surgery_time(OpID, TotalTime) :- 
               surgery_id(OpID, OpCode),  % Mapeia o ID para o tipo de cirurgia
               surgery(OpCode, TAnesthesia, TSurgery, TCleaning),
               TotalTime is TAnesthesia + TSurgery + TCleaning,
               write('Tempo total para a cirurgia (ID '), write(OpID), write('): '), write(TotalTime), nl.
               
               
               
             can_schedule_surgery(Doctor, Day, OpID) :- 
                 write('Verificando se o médico pode agendar a cirurgia (ID '), write(OpID), write('): '), write(Doctor), nl,
             
                 % Obtém o tempo total necessário para a cirurgia
                 get_surgery_time(OpID, TotalTime),  
                 write('Tempo total para a cirurgia (ID '), write(OpID), write('): '), write(TotalTime), nl,
             
                 % Obtém o horário de trabalho do médico
                 timetable(Doctor, Day, (WorkStart, WorkEnd)),  
                 write('Horário de trabalho do médico: '), write((WorkStart, WorkEnd)), nl,
             
                 % Obtém a agenda do médico e calcula os horários livres
                 agenda_staff(Doctor, Day, Agenda),  
                 write('Agenda do médico: '), write(Agenda), nl,
             
                 % Calcula os períodos livres na agenda do médico
                 availability(Doctor, Day ,FreeTimes),
                 
                 write('Períodos livres na agenda: '), write(FreeTimes), nl,
             
                 % Verifica se existe algum período livre suficiente para a cirurgia
                 find_available_time_for_surgery(FreeTimes, TotalTime, StartTime),
                 
                 % Converte o horário de início para o formato de horas e minutos
                 minutes_to_time_format(StartTime, Hour, Minute),
                 
                 write('Médico '), write(Doctor), write(' pode realizar a cirurgia no horário: '),
                 write(Hour), write(':'), write(Minute), nl.
             
             % Encontrar um horário disponível para a cirurgia
             find_available_time_for_surgery([], _, _) :- 
                 fail.  % Caso não haja horário disponível
             
             find_available_time_for_surgery([(Start, End) | Rest], TotalTime, StartTime) :-
                 % Verifica se o intervalo é grande o suficiente para a cirurgia
                 (End - Start >= TotalTime ->
                     StartTime = Start  % Se o intervalo for suficiente, usa o horário de início
                 ;
                     % Caso contrário, verifica o próximo intervalo
                     find_available_time_for_surgery(Rest, TotalTime, StartTime)
                 ).
             
             % Converte o horário de minutos para horas e minutos
             minutes_to_time_format(Minutes, Hour, Minute) :-
                 Hour is Minutes // 60,
                 Minute is Minutes mod 60.
    
    