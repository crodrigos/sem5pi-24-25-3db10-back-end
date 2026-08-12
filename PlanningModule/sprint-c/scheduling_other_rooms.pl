:- dynamic room_distribution/2.

surgery(so2, 45, 60, 45).  
surgery(so3, 45, 90, 45). 
surgery(so4, 45, 75, 45). 

surgery_id(so100001, so2).
surgery_id(so100002, so3).
surgery_id(so100003, so4).
surgery_id(so100004, so2).
surgery_id(so100005, so4).
surgery_id(so100006, so2).
surgery_id(so100007, so3).
surgery_id(so100008, so4).
surgery_id(so100009, so2).
surgery_id(so100010, so3).
surgery_id(so100011, so4).
surgery_id(so100012, so2).
surgery_id(so100013, so3).
surgery_id(so100014, so4).
surgery_id(so100015, so2).
surgery_id(so100016, so3).
surgery_id(so100017, so4).
surgery_id(so100018, so2).
surgery_id(so100019, so3).
surgery_id(so100020, so4).
surgery_id(so100021, so2).
surgery_id(so100022, so3).
surgery_id(so100023, so4).
surgery_id(so100024, so2).
surgery_id(so100025, so3).
surgery_id(so100026, so4).
surgery_id(so100027, so2).
surgery_id(so100028, so3).
surgery_id(so100029, so4).
surgery_id(so100030, so2).
surgery_id(so100031, so3).
surgery_id(so100032, so4).
surgery_id(so100033, so2).
surgery_id(so100034, so3).
surgery_id(so100035, so4).
surgery_id(so100036, so2).
surgery_id(so100037, so3).
surgery_id(so100038, so4).
surgery_id(so100039, so2).
surgery_id(so100040, so3).
surgery_id(so100041, so4).
surgery_id(so100042, so2).
surgery_id(so100043, so3).
surgery_id(so100044, so4).
surgery_id(so100045, so2).
surgery_id(so100046, so3).
surgery_id(so100047, so4).
surgery_id(so100048, so2).
surgery_id(so100049, so3).
surgery_id(so100050, so4).
surgery_id(so100051, so2).
surgery_id(so100052, so3).
surgery_id(so100053, so4).
surgery_id(so100054, so2).
surgery_id(so100055, so3).
surgery_id(so100056, so4).
surgery_id(so100057, so2).
surgery_id(so100058, so3).
surgery_id(so100059, so4).
surgery_id(so100060, so2).
surgery_id(so100061, so3).
surgery_id(so100062, so4).
surgery_id(so100063, so2).
surgery_id(so100064, so3).
surgery_id(so100065, so4).
surgery_id(so100066, so2).
surgery_id(so100067, so3).
surgery_id(so100068, so4).
surgery_id(so100069, so2).
surgery_id(so100070, so3).
surgery_id(so100071, so4).
surgery_id(so100072, so2).
surgery_id(so100073, so3).
surgery_id(so100074, so4).
surgery_id(so100075, so2).
surgery_id(so100076, so3).
surgery_id(so100077, so4).
surgery_id(so100078, so2).
surgery_id(so100079, so3).
surgery_id(so100080, so4).
surgery_id(so100081, so2).
surgery_id(so100082, so3).
surgery_id(so100083, so4).
surgery_id(so100084, so2).
surgery_id(so100085, so3).
surgery_id(so100086, so4).
surgery_id(so100087, so2).
surgery_id(so100088, so3).
surgery_id(so100089, so4).
surgery_id(so100090, so2).
surgery_id(so100091, so3).
surgery_id(so100092, so4).
surgery_id(so100093, so2).
surgery_id(so100094, so3).
surgery_id(so100095, so4).
surgery_id(so100096, so2).
surgery_id(so100097, so3).
surgery_id(so100098, so4).
surgery_id(so100099, so2).
surgery_id(so100100, so3).

surgeries(5).

assignment_surgery(so100001,d001).
assignment_surgery(so100002,d002).
assignment_surgery(so100003,d003).
assignment_surgery(so100004,d001).
assignment_surgery(so100005,d002).

agenda_operation_room(or1,20241028,[(520,579,so100000),(1000,1059,so099999)]).
agenda_operation_room(or2,20241028,[]).
agenda_operation_room(or3,20241028,[]).
agenda_operation_room(or4,20241028,[]).
agenda_operation_room(or5,20241028,[]).
agenda_operation_room(or6,20241028,[]).

remove_duplicates([],[]).
remove_duplicates([H|T],[H|T1]):-
     \+ member(H,T),  
     remove_duplicates(T,T1).
remove_duplicates([H|T],T1):-
    member(H,T),  
    remove_duplicates(T,T1).  

surgeries_length(Size):-
    findall(Surgery,surgery_id(Surgery,_),Surgeries),
    length(Surgeries,Size).
    
rooms_length(Date,RoomCount) :-
    findall(Room,(agenda_operation_room(Room,Date,_)),AvailableRooms),
    length(AvailableRooms,RoomCount).
    
get_surgery_total_time(SurgeryID, Time) :-
    surgery_id(SurgeryID,Type),        
    surgery(Type, Time1, Time2, Time3),
    Time is Time1 + Time2 + Time3.       
    
get_all_surgeries_with_time(SurgeriesL) :-
    findall([SurgeryID, TTime], get_surgery_total_time(SurgeryID, TTime), SurgeriesWithTime),
    sort(2, @=<, SurgeriesWithTime, SortedSurgeries),  % Ordena pela coluna 2 (tempo total)
    findall(SurgeryID, member([SurgeryID,_], SortedSurgeries), SurgeriesL).

get_room_occupation(Date,Room,0) :-
    agenda_operation_room(Room, Date, []).
get_room_occupation(Date,Room,Count) :-
    agenda_operation_room(Room,Date,Op),
    length(Op,Count).
    
get_all_rooms_with_occupation(Date,RoomsOccupationL):-
    findall([Room,RSize],get_room_occupation(Date,Room,RSize),RoomsOccupationL1),
    remove_duplicates(RoomsOccupationL1,RoomsOccupationL2),!,
    sort(2,@>=,RoomsOccupationL2,RoomsOccupationL).
    
calculate_average_distribution(RL,SL,AvgDis) :- 
    (RL > 0 -> AvgDis is ceiling(SL/RL)  
    ; 
        AvgDis = 0  
    ).

rooms_distributions(Date) :-
    rooms_length(Date, RL),
    surgeries_length(SL),
    calculate_average_distribution(RL, SL, AvgDis),
    get_all_rooms_with_occupation(Date, RoomsOccupationL),
    get_all_surgeries_with_time(SurgeriesWithTimeL),
    rooms_distributions1(RoomsOccupationL, SurgeriesWithTimeL, AvgDis),
    !.

rooms_distributions1([],[],_).
rooms_distributions1([],_,_).
rooms_distributions1(_,[],_).
rooms_distributions1([[Room,_]|RRest],Surgeries,AvgDis) :-
    assign_operations_to_room(Room,Surgeries,AvgDis,UpdatedRest,_),
    rooms_distributions1(RRest,UpdatedRest,AvgDis).

assign_operations_to_room(_,[],0,[],NewList) :- assert(room_distribution(_, NewList)).
assign_operations_to_room(Room,[],_,[],NewList) :- assert(room_distribution(Room, NewList)).
assign_operations_to_room(Room, [Surgery|SRest], AvgDis1, UpdatedRest, NewList) :-
   append(NewList, [Surgery], UpdatedList),
   AvgDis is AvgDis1 - 1,
    
    ( AvgDis = 0
    ->  assert(room_distribution(Room,UpdatedList)),
        UpdatedRest = SRest,
        !
    ;   assign_operations_to_room(Room,SRest,AvgDis,UpdatedRest,UpdatedList),!
    ).