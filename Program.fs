open System
open System.IO
open System.Text.RegularExpressions

// Function to clean and filter out punctuation, keeping F# as a special case
let cleanText (text: string) =
    // Keep "F#" as a special case and remove other punctuation
    let cleaned = Regex.Replace(text, @"[^\w\s#]", "")
    cleaned

// Function to read text from file or console
let getTextInput () =
    printfn "Choose input method: 1 - File, 2 - Direct input"
    match Console.ReadLine() with
    | "1" -> 
        printfn "Enter the file path:"
        let filePath = Console.ReadLine()
        if File.Exists filePath then
            File.ReadAllText(filePath)
        else
            printfn "File not found. Exiting."
            ""
    | "2" -> 
        printfn "Enter your text:"
        Console.ReadLine()
    | _ -> 
        printfn "Invalid choice. Exiting."
        ""

// Function to analyze text
let analyzeText (text: string) =
    let cleanedText = cleanText text
    let words = cleanedText.Split([| ' '; '\n'; '\t' |], StringSplitOptions.RemoveEmptyEntries)
    let sentences = text.Split([| '.'; '!' |], StringSplitOptions.RemoveEmptyEntries)
    let paragraphs = text.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)

    let wordCount = words.Length
    let sentenceCount = sentences.Length
    let paragraphCount = paragraphs.Length

    let wordFrequency =
        words
        |> Seq.map (fun word -> word.ToLowerInvariant())
        |> Seq.countBy id
        |> Seq.sortByDescending snd

    let averageSentenceLength =
        if sentenceCount > 0 then
            float wordCount / float sentenceCount
        else
            0.0

    // Return analysis results
    wordCount, sentenceCount, paragraphCount, wordFrequency, averageSentenceLength

// Function to display analysis results
let displayResults (wordCount, sentenceCount, paragraphCount, wordFrequency, avgSentenceLength) =
    printfn "Text Analysis Results:"
    printfn "-----------------------"
    printfn "Total Words: %d" wordCount
    printfn "Total Sentences: %d" sentenceCount
    printfn "Total Paragraphs: %d" paragraphCount
    printfn "Average Sentence Length: %.2f words" avgSentenceLength
    printfn "\nWord Frequency (Top 5):"
    wordFrequency
    |> Seq.take 5
    |> Seq.iter (fun (word, count) -> printfn "  %s: %d" word count)

// Main function
[<EntryPoint>]
let main argv =
    let text = getTextInput()
    if text <> "" then
        let analysisResults = analyzeText text
        displayResults analysisResults
    0 // Return exit code
