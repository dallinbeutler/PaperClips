module Manufacturing
open Fable.React.Standard
open Fable.React.Helpers
open Elmish

type Model ={
    clips: int
    clipsPerSecond: int
    wireLeft: int
    wireCost : float
    wirePriceTicks: float
    wireBasePrice:float
}

type Msg =
|Tick
|BoughtMoreWire
|Increment

let init()= 
    {
        clips = 0
        clipsPerSecond = 0
        wireLeft = 0
        wireCost = 10.0
        wirePriceTicks = 0.0
        wireBasePrice = 0.12
    }
let r = System.Random()
let adjustWirePrice model = 
    
    let wireBPrice = if (model.wirePriceTicks> 250.0 && model.wireBasePrice>15.0)
                     then model.wireBasePrice * 0.999
                     else model.wireBasePrice
    if (r.NextDouble() < 0.015 )
    then
        let wireAdjust = 6.0 * (sin model.wirePriceTicks)
        let wireCost = ceil (wireBPrice + wireAdjust)
        {model with wirePriceTicks = model.wirePriceTicks + 2.0
                    wireBasePrice  = wireBPrice
                    wireCost       = wireCost}        
    else
        {model with wirePriceTicks = model.wirePriceTicks + 1.0}

let update msg model = 
    match msg with 
    |BoughtMoreWire -> {model with wireLeft = model.wireLeft + 1000}
                        , Shared.PurchaseRequested model.wireCost
    |Tick -> adjustWirePrice model, Shared.Nop
    |Increment -> {model with clips = model.clips + 1
                              wireLeft = model.wireLeft - 1}, Shared.ClipCreated

open Fable.React
open Fable.React.Props
let view state dispatch money=
    div[] [
        h2[][ str "Manufacturing"]
        hr[]
        p [][str (sprintf "Clips per Second: %i" state.clipsPerSecond )]
        p[][
            button [Disabled (state.wireCost > money)  ;OnClick(fun _-> dispatch BoughtMoreWire ) ][str "wire"]
            label [][str (sprintf " %i inches" state.wireLeft )]
        ]
        p [][str (sprintf " Cost: $ %0.00f" state.wireCost)]
        br[]
        

    ]