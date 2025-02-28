using HtmlAgilityPack;

namespace ConsoleApp_DD_Scruping;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello to DD Scraping!");
        
        var path = $"C:\\Users\\monl\\Desktop\\MyTest\\AgilityDD\\";
        //var docToLoadTitle = $"ELENCO ALFABETICO SEGNALAZIONI SERIE TV.htm";
        var docToLoadTitle = $"100 Code - Stagione 1 [COMPLETA] _ ddunlimited.net _ Standard Definition ITA • ddunlimited.net.htm";

Console.WriteLine("============ Single page ==============");

        // Specifica il percorso del file HTML caricato
        //string filePath = "Dexter_ Original Sin - Stagione 1 [COMPLETA] _ ddunlimited.net _ Standard Definition ITA • ddunlimited.net.html";
       //string filePath = $"C:\\Users\\monl\\Desktop\\MyTest\\AgilityDD\\100 Code - Stagione 1 [COMPLETA] _ ddunlimited.net _ Standard Definition ITA • ddunlimited.net.htm";
       string filePath = $"C:\\Users\\monl\\Desktop\\MyTest\\AgilityDD\\Love Me Licia - Stagione 1 [32_34] _ ddunlimited.net _ Standard Definition ITA • ddunlimited.net.htm";
       
        // Verifica l'esistenza del file
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Il file '{filePath}' non esiste.");
            return;
        }
        
        // Legge il contenuto del file HTML
        string htmlContent = File.ReadAllText(filePath);
        
                // Carica il contenuto nel documento HtmlAgilityPack
        HtmlDocument _doc = new HtmlDocument();
        _doc.LoadHtml(htmlContent);
        
        //cerca il titolo
         var mainTitleNode = _doc.DocumentNode.SelectSingleNode("//title");
        if (mainTitleNode == null)
        {
            Console.WriteLine("Tag <title> non trovato.");
            return;
        }
        
        // Stampa il valore (il testo) del tag <title>
        string mainTitle = mainTitleNode.InnerText.Trim();
        Console.WriteLine($"Valore del tag <title>: {mainTitle}");
        
        Threads _mytThread = new Threads
        {
           Id = 1, MainTitle = mainTitle, Type = "Tv Serie", Url = filePath, CreatedDate = DateTime.Now, IsActive = true
        };
        
        Console.WriteLine($"Creato nuovo record Threads: {_mytThread.Id} - {_mytThread.MainTitle} - {_mytThread.Type} - {_mytThread.Url}");
        
        // Seleziona tutti i tag <a> che hanno l'attributo title="Aggiungi a Emule"
        var titleNodes = _doc.DocumentNode.SelectNodes("//a[@title='Aggiungi a Emule']");
        if (titleNodes == null || titleNodes.Count == 0)
        {
            Console.WriteLine("Nessun tag <a> con title 'Aggiungi a Emule' trovato.");
            return;
        }
        
        // Stampa la tabella: colonna "Titolo" (il contenuto del tag) e "Link ed2k" (il valore href del tag <a> immediatamente successivo)
        Console.WriteLine("{0,-40} | {1}", "Titolo", "Link ed2k");
        Console.WriteLine(new string('-', 90));
        
        foreach (var titleNode in titleNodes)
        {
            // Il titolo è il contenuto del tag <a> con title "Aggiungi a Emule"
            string titolo = titleNode.InnerText.Trim();
            
            // Cerca il tag <a> immediatamente successivo (fratello)
            var nextANode = titleNode.SelectSingleNode("following-sibling::a[1]");
            if (nextANode != null)
            {
                string linkEd2k = nextANode.GetAttributeValue("href", "Nessun href").Trim();
                Console.WriteLine("{0,-40} | {1}", titolo, linkEd2k);
            }
            else
            {
                Console.WriteLine("{0,-40} | {1}", titolo, "Nessun tag <a> successivo trovato");
            }
        }
        
        
        
        
        
        Console.WriteLine("================");
        Console.ReadLine();
        return;
        var doc = new HtmlDocument();
        doc.Load($"{path}{docToLoadTitle}");
        Console.WriteLine($"Document Title: {doc.DocumentNode.SelectSingleNode("//title").InnerHtml}");
        
        
        Console.WriteLine($"Document Title: {doc.DocumentNode.SelectSingleNode(".//a[@title='Aggiungi a Emule']\"").InnerHtml}");

         // Trova il nodo <span> con stile "font-size: 280%; line-height: 116%;" e testo "SD - Definizione Standard"
        var sdSpan = doc.DocumentNode.SelectSingleNode("//span[contains(@style, 'font-size: 280%') and contains(@style, 'line-height: 116%') and contains(., 'SD - Definizione Standard')]");
        if (sdSpan == null)
        {
            Console.WriteLine("Nodo 'SD - Definizione Standard' non trovato.");
            return;
        }

        // Trova il nodo <span> successivo con stile "text-decoration: underline" e testo "Segnalazioni in Italiano"
        var segSpan = sdSpan.SelectSingleNode("following::span[contains(@style, 'text-decoration: underline') and contains(., 'Segnalazioni in Italiano')]");
        if (segSpan == null)
        {
            Console.WriteLine("Nodo 'Segnalazioni in Italiano' non trovato.");
            return;
        }

        // Seleziona tutti i tag <ul> successivi al nodo 'Segnalazioni in Italiano'
        var ulNodes = segSpan.SelectNodes("following::ul");
        if (ulNodes == null || ulNodes.Count == 0)
        {
            Console.WriteLine("Nessun tag <ul> trovato dopo 'Segnalazioni in Italiano'.");
            return;
        }

        // Dalle <ul> selezionate, estrai tutti i tag <a class="postlink-local">
        List<HtmlNode> anchorNodes = new List<HtmlNode>();
        foreach (var ul in ulNodes)
        {
            var anchors = ul.SelectNodes(".//a[@class='postlink-local']");
            if (anchors != null)
            {
                anchorNodes.AddRange(anchors);
            }
        }

        if (anchorNodes.Count == 0)
        {
            Console.WriteLine("Nessun link <a class=\"postlink-local\"> trovato nelle <ul> selezionate.");
            return;
        }

        // Per ciascun link, estrae il testo diretto (escludendo eventuali <span> interni) e il valore di href
        List<(string Nome, string Link)> elementi = new List<(string, string)>();
        foreach (var anchor in anchorNodes)
        {
            var textNodes = anchor.SelectNodes("text()");
            string nome = textNodes != null 
                          ? string.Join(" ", textNodes.Select(n => n.InnerText.Trim()))
                          : string.Empty;
            string link = anchor.GetAttributeValue("href", "").Trim();
            elementi.Add((nome, link));
        }

        // Stampa il risultato in una tabella Nome / Link
        Console.WriteLine("{0,-30} | {1}", "Nome", "Link");
        Console.WriteLine(new string('-', 70));
        foreach (var item in elementi)
        {
            Console.WriteLine("{0,-30} | {1}", item.Nome, item.Link);
        }
        
        Console.ReadLine();
    }
    
}