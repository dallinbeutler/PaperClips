module Manufacturing
open Fable.React.Standard
open Fable.React.Helpers

type Model ={
    clipsPerSecond: int
    wireLeft: int
    cost : int
    purchasePrice : float
}

type Msg =
|Tick
|BuyMoreWire

let init()= 
    {
        clipsPerSecond = 0
        wireLeft = 0
        cost = 0
        purchasePrice = 0.0
    }


let update msg model = 
    match msg with 
    |Tick ->model
    |BuyMoreWire -> {model with wireLeft = model.wireLeft + 1000}

open Fable.React
open Fable.React.Props
let view state dispatch =
    div[] [
        h2[][ str "Manufacturing"]
        hr[]
        p [][str (sprintf "Clips per Second: %i" state.clipsPerSecond )]
        p[][
            button [ OnClick(fun _-> dispatch BuyMoreWire ) ][str "wire"]
            label [][str (sprintf " %i inches" state.wireLeft )]
        ]
        p [][str (sprintf " Cost: $ %i" state.cost)]
        br[]
        

    ]