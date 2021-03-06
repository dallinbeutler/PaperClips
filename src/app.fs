module App
open Elmish
type Model = 
  {
    business : Business.Model
    manu : Manufacturing.Model
  }

type Msg =
| Business of Business.Msg
| Manufact of Manufacturing.Msg
| Tick
//| TextChange

let init() = 
  {
    business =  Business.init()
    manu = Manufacturing.init()
  },Cmd.none
open Fable.React
open Fable.React.Props



let handleShared  (msg:Shared.Msg) (state:Model)=
  match msg with
  |Shared.Nop-> state
  |Shared.ClipCreated -> {state with business = {state.business with unsold = state.business.unsold + 1 }}
  |Shared.PurchaseRequested amount-> {state with business = Business.buy state.business amount} 

// UPDATE
let update (msg:Msg) (state:Model) :Model * Cmd<Msg>=
    let buy cost= 
      Business.buy state.business cost
    printfn "Update!"
    //<@state.curAmount@>
    match msg with
    | Business b ->{state with business = Business.update b state.business},Cmd.none
    | Manufact man-> let (model,sharedMsg) =((Manufacturing.update) man (state.manu))
                     {state with manu =model; }|> handleShared sharedMsg, Cmd.none
    | Tick->let (man,shared) = Manufacturing.update Manufacturing.Tick state.manu 
            {state with business = Business.update Business.TimeTick state.business
                        manu = man}|> handleShared shared,Cmd.none//{state with curAmount = state.curAmount + 1},Cmd.none 
    //| TextChange -> printfn "Text change!"; {state with curAmount = state.curAmount + state.raiseAmount},Cmd.none

let thousands(x:string) = 
  let mutable it = 0
  String.collect (
    fun c -> 
    it<-it+1
    if (it <> x.Length) && ((it - (x.Length)) % 3 = 0)
    then string c + ","
    else string c) x

// VIEW (rendered with React)
let view state dispatch =
  let toDispatch converter msg = 
    dispatch (converter msg)
  //let myeditor = div[ContentEditable true; Style[BorderColor 0x0;BorderWidth 2; BorderStyle "solid"; Padding 4]; OnInput(fun ev-> dispatch TextChange) ][str "Edit me plz  i";b[][str "fun"]; str " stuff"]
  div [ Style[ Width 300] ]
      [ 
        //myeditor
        //h2 [] [ str (sprintf "Paperclips: %A" state.curAmount) ]
        h2 [] [ str (sprintf "Paperclips: %A" (thousands (string state.manu.clips))) ]
        button [Disabled (state.manu.wireLeft = 0) ;OnClick (fun _ -> dispatch (Manufact Manufacturing.Msg.Increment)) ] [ str "make Paperclip" ] 
        Business.view state.business (toDispatch Business) 
        Manufacturing.view (state.manu) (toDispatch Manufact) state.business.funds
      ]
open Fable.Reaction
open Elmish.React
open System
open FSharp.Control.AsyncRx
open FSharp.Control 
// open System.Reactive.Linq

let ticker (disp:Dispatch<Msg>) = 
  //Async.RunSynchronously ((AsyncRx.interval 10 10).SubscribeAsync(fun t -> async{disp (Tick)} ))
  let rec infiniteHello() =
    async {
        do! Async.Sleep 100
        disp Tick
        return! infiniteHello()
    }
  infiniteHello() |> Async.Start
  ()
  //let event = Async.AwaitEvent (t.Elapsed)
  // Observable.Timer(TimeSpan.FromSeconds 0.5).Subscribe(fun t-> disp Tick)|>ignore
  // let sw = new System.Diagnostics.Stopwatch()
  // sw
  // let t = new System.Threading.Timer(fun cb -> ())
  // t.
  
  
// App
Program.mkProgram init update view
|> Program.withReactBatched "elmish-app"
|> Program.withConsoleTrace
|> Program.withSubscription (fun m -> Cmd.ofSub ticker )
|> Program.run