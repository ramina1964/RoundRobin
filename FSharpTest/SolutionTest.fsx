open System

type Resultater = System.Collections.Generic.HashSet<int * int>
let res = Resultater()

let add (x,y) =
    res.Add(x,y) |>ignore
    res.Add(y,x) |> ignore

let canPlay (a,b) = 
    res.Contains (a,b) |> not    

let tryRotate lst = 
    match lst with
    |x::y::t ->
        match List.tryFind (fun e -> e > y) t with
        |Some r -> 
            let rem = List.filter (fun e -> e <> r) (y::t) |> List.sort
            let res = x::r::rem
            Some res
        |_ -> None
    |_ -> None

let rec tryGetNext acc lst = //nøster seg tilbake til og prøver med nytt par
    match tryRotate lst with
    |Some li -> Some (li, acc)
    |_ -> 
        match acc with
        |(a,b)::t -> 
            let ret = a::b::(lst|>List.sort)
            tryGetNext t ret 
        |_ -> None

let settopp list =
    let rec loop lst acc =
        match lst with
        |x::y::rest ->
            match canPlay (x,y) with
            | true -> loop rest ((x,y) :: acc)
            | _ ->
                match tryGetNext acc lst with
                |Some (l,ac) -> loop l ac
                |_ -> None
        | x::t -> None
        | _ -> 
            let akk = acc |> List.rev
            akk|>List.iter add
            Some akk
    loop list []
       
#time

let allerunder list =
    res.Clear()
    for i in list do
        settopp list |> printfn "%A"    

allerunder [1..26]

//etterprosessering av resultater dersom runden ikke går opp

let findOpponents id = [1..26] |> List.filter (fun x -> if id = x then false else res.Contains(id,x) |> not)

let findCirclePair id = 
    let rec loop acc =
        match acc with
        |x::y::t ->
            let op = findOpponents x |> List.except [y] |> List.head
            if op = id then 
                acc |> List.rev
            else 
                loop (op::acc)       
        |h::t ->
            let op = findOpponents h |> List.head            
            loop (op::acc)
        |_ -> acc
    loop [id]

let res1 = 
    let list = findCirclePair 1
    let lengde = list.Length
    if lengde % 2 = 1 then
        printfn "Oddetallsgruppe funnet - runden er ikke mulig å sette opp. Lengden på gruppen er: %i \n" lengde
        list |> List.iter (printf "%i -> ")
        printfn ""

