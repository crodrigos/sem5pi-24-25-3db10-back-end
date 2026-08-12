:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_json)).
:- use_module(library(http/http_parameters)).
:- use_module(library(lists)).
:- use_module(library(http/http_cors)).

:- set_prolog_flag(encoding, utf8).

% Cors: Permitir requisições de qualquer origem
:- set_setting(http:cors, [*]).

% Rota para calcular a melhor solução
:- http_handler(root(calcular_better_sol), handle_obtain_better_sol, []).

% Predicado para processar a requisição de /calcular_better_sol
handle_obtain_better_sol(Request) :-
    % Ativa CORS para a requisição
    cors_enable,

    % Processa os parâmetros da requisição HTTP
    http_parameters(Request, [
        room(Room, [atom]),   % Recebe a sala como um átomo
        day(Day, [integer])   % Recebe o dia como um inteiro
    ]),

    % Chama o predicado obtain_better_sol/7 com os parâmetros recebidos
    obtain_better_sol(Room, Day, AgOpRoomBetter, LAgDoctorsBetter, LAgNursesBetter, LCleaning, TFinOp),

    % Converte listas complexas para JSON
    convert_segment_list_to_json(AgOpRoomBetter, JsonAgOpRoomBetter),
    convert_doctors_list_to_json(LAgDoctorsBetter, JsonLAgDoctorsBetter),
    convert_doctors_list_to_json(LAgNursesBetter, JsonLAgNursesBetter),
    convert_doctors_list_to_json(LCleaning, JsonLCleaning),

    % Prepara a resposta JSON
    reply_json_dict(_{
        status: "success",
        room: Room,
        day: Day,
        ag_op_room_better: JsonAgOpRoomBetter,
        ag_doctors_better: JsonLAgDoctorsBetter,
        ag_nurses_better: JsonLAgNursesBetter,
        cleaning_schedule: JsonLCleaning,
        final_time: TFinOp
    }, [encoding(utf8)]).

% Predicados auxiliares para conversão de listas complexas para JSON
convert_segment_list_to_json([], []).
convert_segment_list_to_json([(Start, End, OpCode) | Tail], [Reply | JsonArray]) :-
    Reply = _{start: Start, end: End, operation: OpCode},
    convert_segment_list_to_json(Tail, JsonArray).

convert_doctors_list_to_json([], []).
convert_doctors_list_to_json([(Doctor, Agenda) | Tail], [Reply | JsonArray]) :-
    convert_segment_list_to_json(Agenda, JsonAgenda),
    atom_string(Doctor, DoctorS),
    Reply = _{doctor: DoctorS, agenda: JsonAgenda},
    convert_doctors_list_to_json(Tail, JsonArray).
 