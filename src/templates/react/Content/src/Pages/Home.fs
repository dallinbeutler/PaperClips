namespace FableElmishReactTemplate

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open FableElmishReactTemplate.Common

module Home =

  type Model = string

  type Msg =
    | ChangeStr of string

  let update msg model =
    match msg with
    | ChangeStr str ->
        str, Cmd.Empty

  open Fable.Helpers.React
  open Fable.Helpers.React.Props

  let view model dispatch =
    div
      [ ]
      [
        p
          [ ClassName "control"
          ]
          [ input
              [ ClassName "input"
                Type "text"
                Placeholder "Type your name"
                DefaultValue !^model
                AutoFocus true
                OnChange (fun ev -> !!ev.target?value |> ChangeStr |> dispatch )
              ] [ ]
          ]
        br [] [ ]
        span
          []
          [ str (sprintf "Hello %s" model) ]
      ]
