module main

open System
open OpenQA.Selenium
open canopy
open runner
open configuration
open reporters
open types

start firefox
let mainBrowser = browser
elementTimeout <- 3.0
compareTimeout <- 3.0
pageTimeout <- 3.0
runFailedContextsFirst <- true
//reporter <- new LiveHtmlReporter(Chrome, configuration.chromeDir) :> IReporter 

failFast := true

//login
url "https://uat.iq.brightline.tv"
"#EmailAddress" << "uitester@mailinator.com"
"#Password" << "Digital6!"
click ".btn-success"

//load all modules
//promotionalCreative.all()
//destinationCreative.all()
//campaign.all()
ad.all()

//run all tests from all modules
run ()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()