:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_json)).
:- use_module(library(http/json)).

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

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Registra os handlers.
:- http_handler(root(receber_dados), receber_dados, []).  % Para receber cirurgia, staff, timetable, surgery_id, agenda_staff e agenda_operation_room

% Rota para consultar os dados armazenados
:- http_handler(root(consultar_cirurgias), consultar_cirurgias, []).
:- http_handler(root(consultar_staff), consultar_staff, []).
:- http_handler(root(consultar_timetable), consultar_timetable, []).
:- http_handler(root(consultar_surgery_id), consultar_surgery_id, []).
:- http_handler(root(consultar_agenda_staff), consultar_agenda_staff, []).
:- http_handler(root(consultar_assignment_surgery), consultar_assignment_surgery, []).
:- http_handler(root(consultar_agenda_operation_room), consultar_agenda_operation_room, []).

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Converte o tempo no formato "HH:MM:SS" para minutos
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
    http_read_json_dict(Pedido, DadosRecebidos),
    (   is_dict(DadosRecebidos)
    ->  processar_dados(DadosRecebidos),
        reply_json_dict(_{status: "sucesso", mensagem: "Dados armazenados com sucesso."})
    ;   reply_json_dict(_{status: "erro", mensagem: "JSON inválido."}, [status(400)])).

processar_dados(Dados) :-
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
    convert_to_atom(Item.roomNumber, RoomNumber),
    number_string(Date, Item.date),  % Converte a string para número
    maplist(parse_room_schedule, Item.schedule, ParsedSchedule),
    assertz(agenda_operation_room(RoomNumber, Date, ParsedSchedule)).

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