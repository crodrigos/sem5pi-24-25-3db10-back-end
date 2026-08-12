:-dynamic generations/1.
:-dynamic population/1.
:-dynamic prob_crossover/1.
:-dynamic prob_mutation/1.

:-dynamic original_fitness/2.
:-dynamic stabilization_limit/1.
:-dynamic time_limit/1.

:-ensure_loaded('helper.pl').

initialize:-	
    (retract(generations(_));true), asserta(generations(100)),		
	(retract(population(_));true), asserta(population(3)),
    (retract(prob_crossover(_));true), asserta(prob_crossover(0.5)),
    (retract(prob_mutation(_));true), asserta(prob_mutation(0.8)),
    (retract(stabilization_limit(_));true), asserta(stabilization_limit(3)),
    (retract(evaluation_desired(_));true), asserta(evaluation_desired(1000)).
               
generate:-
    initialize,
    generate_population(Pop),
    generate_generation(0,0,Pop),
    !.
    
generate_population(Pop):-
    population(PopSize), 
    findall(Surgery,surgery_id(Surgery,_),SurgeriesList),
    length(SurgeriesList,NumSurgeries),
    generate_population(PopSize,SurgeriesList,NumSurgeries, Pop). 

generate_population(0,_,_,[]):-!.
generate_population(PopSize,SurgeriesList,NumSurgeries,[Ind|Rest]):-
    PopSize1 is PopSize-1,
    generate_population(PopSize1,SurgeriesList,NumSurgeries,Rest),
    generate_individual(SurgeriesList,NumSurgeries,Ind),
    not(member(Ind,Rest)).
generate_population(PopSize,SurgeriesList,NumSurgeries,L):-
    generate_population(PopSize,SurgeriesList,NumSurgeries,L).
    
generate_individual([G],1,[G]):-!.
generate_individual(SurgeriesList,NumSurgeries,[G|Rest]):-
    NumTemp is NumSurgeries + 1, 
    random(1,NumTemp,N),
    remove(N,SurgeriesList,G,NewList),
    NumSurgeries1 is NumSurgeries-1,
    generate_individual(NewList,NumSurgeries1,Rest).
    
remove(1,[G|Rest],G,Rest).
remove(N,[G1|Rest],G,[G1|Rest1]):- N1 is N-1,
            remove(N1,Rest,G,Rest1).
            
evaluate_population([],[]).
evaluate_population([Ind|Rest],[Ind,Fit|Rest1]):-
    evaluate(Ind,Fit),
    evaluate_population(Rest,Rest1).
    
evaluate(Ind,Fit):-
    schedule_all_surgeries(or1,20241028,Ind,Fit). 
    
generate_generation(N,NRepeat,Pop):-
    generations(GN),          
    stabilization_limit(ST),  
    evaluation_desired(ED),  
    ( N =:= GN -> 
        write('Reached Maximum Generations '),write(N),nl,
        finalize(Pop)
    ;
        ( NRepeat >= ST -> 
            write('Population has stabilized for '),write(ST),nl,
            finalize(Pop)
        ;
            write('Generation '), write(N), write(':'), nl, write(Pop), nl,
            crossover(Pop,CPop),  
            mutation(CPop,MPop),  
            calculate_top_p(P),
            select_top_p(P,Pop,MPop,TopP,RemainedList,TopPFitness),
            
            ( TopPFitness =< ED -> 
                write('Found a solution with fitness <= '),write(ED),write('. Terminating.'),nl,
                finalize(Pop)
            ;
               not_elitist_selection(P,RemainedList,RemainedTopP),
                append_lists(TopP, RemainedTopP,NewPop),
                
                ( Pop = NewPop -> 
                    NRepeat1 is NRepeat + 1,  
                    N1 is N + 1,
                    generate_generation(N1,NRepeat1,NewPop)  
                ; 
                    N1 is N + 1, 
                    generate_generation(N1,0,NewPop) 
                )
            )
        )
    ).
    
finalize(Pop):-
    get_first_element(Pop,BestInd),
    evaluate_population([BestInd],BestIndValue),
    write('Successfully finished genetic algorithm'),nl,
    write('Best Individual: '),write(BestIndValue),nl.
    
get_first_element([First|_],First).

select_top_p(_,[],[],[],_):-!.
select_top_p(P,Pop,MPop,TopP,RemainedList,TopPFitness):-
      append_lists(Pop,MPop,AList),
      evaluate_population(AList,EvalList),
      sort_population_by_fitness(EvalList,SAListValue),
      take_top_p(P,SAListValue,TopP1,RemainedList),
      extract_p_individuals(TopP1,TopP),
      extract_top_fitness(TopP1,TopPFitness). 
      
not_elitist_selection(P,RemainedList,RemainedTopP):-
     population(N),
     P1 is N - P,
     roulette_evaluation(RemainedList, RemainingRouletteList),
     take_top_p(P1,RemainingRouletteList,RemainedTopP1,_),
     extract_p_individuals(RemainedTopP1, RemainedTopP).

extract_top_fitness([[_,Fitness]|_],Fitness):-!. 
extract_top_fitness([_,Fitness],Fitness). 
extract_top_fitness([],_):-fail. 

extract_p_individuals([],[]).
extract_p_individuals([Ind,_|Rest],[Ind|RemainedTopP]):-
    extract_p_individuals(Rest,RemainedTopP).  

roulette_evaluation1([],[]).
roulette_evaluation1([IndList,Fitness|Tail],[IndList,RoundedFitness|ResultTail]):-
    random(0.0,1.0,RandomFactor),          
    RandomFitness is Fitness * RandomFactor,
    floor(RandomFitness,RoundedFitness),   
    assert(original_fitness(IndList,Fitness)),
    roulette_evaluation1(Tail,ResultTail).   

roulette_evaluation(RemainingList,RemainingListPSort) :-
    roulette_evaluation1(RemainingList,RemainingListP),  
    sort_population_by_fitness(RemainingListP,RemainingListPSortWithModifiedFitness),
    restore_original_fitness(RemainingListPSortWithModifiedFitness,RemainingListPSort),
    !,
    retractall(original_fitness(_,_)).

restore_original_fitness([],[]).
restore_original_fitness([IndList,_|TailRemaining],[IndList,OriginalFitness|ResultTail]):-
    original_fitness(IndList,OriginalFitness),
    restore_original_fitness(TailRemaining,ResultTail).
    
calculate_top_p(Pres):-
    population(N),
    Temp is N * 0.2,  
    round(Temp,Pres).  
    
take_top_p(P,SAListValue,TopP,Rest):-
    N is P * 2,
    take(N,SAListValue,TopP,Rest).  
    
take(0,Rest,[],Rest):-!.
take(N,[X|T1],[X|T2],Rest):-
    N1 is N-1,
    take(N1,T1,T2,Rest).    
    
sort_population_by_fitness(NextGen,Sorted):-
    convert_to_pairs(NextGen,Pairs),
    sort(2,@=<,Pairs,SortedPairs),
    convert_to_original(SortedPairs,Sorted).

convert_to_pairs([],[]).
convert_to_pairs([Genes,Fitness|Rest],[[Genes,Fitness]|PairsRest]):-
    convert_to_pairs(Rest,PairsRest).

convert_to_original([],[]).
convert_to_original([[Genes,Fitness]|Rest],[Genes,Fitness|OriginalRest]):-
    convert_to_original(Rest, OriginalRest).

append_lists(Pop,NPop,NPopRes):-
     append(Pop,NPop,Combined),  
     remove_duplicates(Combined,NPopRes),  
     !.  

remove_duplicates([],[]).
remove_duplicates([H|T],[H|T1]):-
     \+ member(H,T),  
     remove_duplicates(T,T1).
remove_duplicates([H|T],T1):-
    member(H,T),  
    remove_duplicates(T,T1).    
    
crossover([],[]).
crossover([Ind],[Ind]):-!.
crossover([Ind1,Ind2|Rest],[NInd1,NInd2|Rest1]) :-
    generate_crossover_points(P1,P2),
    	prob_crossover(Pcruz),random(0.0,1.0,Pc),
    	((Pc =< Pcruz,!,
            cross(Ind1,Ind2,P1,P2,NInd1),
    	  cross(Ind2,Ind1,P1,P2,NInd2))
    	;
    	(NInd1=Ind1,NInd2=Ind2)),
    	!,
    	crossover(Rest,Rest1).

fillh([],[]).
fillh([_|R1],[h|R2]):-
	fillh(R1,R2).

sublist(L1,I1,I2,L):-I1 < I2,!,
    sublist1(L1,I1,I2,L).
sublist(L1,I1,I2,L):-sublist1(L1,I2,I1,L).

sublist1([X|R1],1,1,[X|H]):-!, fillh(R1,H).
sublist1([X|R1],1,N2,[X|R2]):-!,N3 is N2 - 1,
	sublist1(R1,1,N3,R2).
sublist1([_|R1],N1,N2,[h|R2]):-N3 is N1 - 1,
		N4 is N2 - 1,
		sublist1(R1,N3,N4,R2).

rotate_right(L,K,L1):- surgeries(N),
	T is N - K,
	rr(T,L,L1).

rr(0,L,L):-!.
rr(N,[X|R],R2):- N1 is N - 1,
	append(R,[X],R1),
	rr(N1,R1,R2).

remove([],_,[]):-!.
remove([X|R1],L,[X|R2]):- 
    not(member(X,L)),!,
    remove(R1,L,R2).
remove([_|R1],L,R2):-
    remove(R1,L,R2).

insert([],L,_,L):-!.
insert([X|R],L,N,L2):-
    surgeries(T),
    ((N>T,!,N1 is N mod T);N1 = N),
    insert1(X,N1,L,L1),
    N2 is N + 1,
    insert(R,L1,N2,L2).

insert1(X,1,L,[X|L]):-!.
insert1(X,N,[Y|L],[Y|L1]):-
    N1 is N-1,
    insert1(X,N1,L,L1).
    
generate_crossover_points(P1,P2):- 
    generate_crossover_points1(P1,P2).
generate_crossover_points1(P1,P2):-
	surgeries(N),
	NTemp is N+1,
	random(1,NTemp,P11),
	random(1,NTemp,P21),
	P11\==P21,!,
	((P11<P21,!,P1=P11,P2=P21);P1=P21,P2=P11).
generate_crossover_points1(P1,P2):-
	generate_crossover_points1(P1,P2).

cross(Ind1,Ind2,P1,P2,NInd11):-
    sublist(Ind1,P1,P2,Sub1),
    surgeries(NumT),
    R is NumT-P2,
    rotate_right(Ind2,R,Ind21),
    remove(Ind21,Sub1,Sub2),
    P3 is P2 + 1,
    insert(Sub2,Sub1,P3,NInd1),
    removeh(NInd1,NInd11).

removeh([],[]).
removeh([h|R1],R2):-!,
    removeh(R1,R2).
removeh([X|R1],[X|R2]):-
    removeh(R1,R2).

mutation([],[]).
mutation([Ind|Rest],[NInd|Rest1]):-
	prob_mutation(Pmut),
	random(0.0,1.0,Pm),
	((Pm < Pmut,!,mutacao1(Ind,NInd));NInd = Ind),
	mutation(Rest,Rest1).

mutacao1(Ind,NInd):-
	generate_crossover_points(P1,P2),
	mutacao22(Ind,P1,P2,NInd).

mutacao22([G1|Ind],1,P2,[G2|NInd]):-
	!, P21 is P2-1,
	mutacao23(G1,P21,Ind,G2,NInd).
mutacao22([G|Ind],P1,P2,[G|NInd]):-
	P11 is P1-1, P21 is P2-1,
	mutacao22(Ind,P11,P21,NInd).

mutacao23(G1,1,[G2|Ind],G2,[G1|Ind]):-!.
mutacao23(G1,P,[G|Ind],G2,[G|NInd]):-
	P1 is P-1,
	mutacao23(G1,P1,Ind,G2,NInd).