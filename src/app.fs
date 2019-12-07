module App
open Elmish
type Model = 
  {
    curAmount : int
    raiseAmount:int
    business : Business.Model
    manu : Manufacturing.Model
  }

type Msg =
| Increment
| Business of Business.Msg
| Manufact of Manufacturing.Msg
| Tick
//| TextChange

let init() = 
  {
    curAmount = 0
    raiseAmount = 1
    business =  Business.init()
    manu = Manufacturing.init()
  },Cmd.none
open Fable.React
open Fable.React.Props

// UPDATE
let update (msg:Msg) (state:Model) =
    printfn "Update!"
    //<@state.curAmount@>
    match msg with
    | Increment -> {state with curAmount = state.curAmount + state.raiseAmount},Cmd.none
    | Business b ->{state with business = Business.update b state.business},Cmd.none
    | Manufact manu->state,Cmd.none 
    | Tick->{state with curAmount = state.curAmount + 1},Cmd.none 
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
        h2 [] [ str (sprintf "Paperclips: %A" state.curAmount) ]
        h2 [] [ str (sprintf "Paperclips: %A" (thousands (string state.curAmount))) ]
        button [ OnClick (fun _ -> dispatch Increment) ] [ str "make Paperclip" ] 
        Business.view state.business (toDispatch Business) 
        Manufacturing.view (state.manu) (toDispatch Manufact)
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
        do! Async.Sleep 1000
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