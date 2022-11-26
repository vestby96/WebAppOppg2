function sortTable(n) { // funksjon som sorterer en kolonne, n = kolonnen som skal sorteres
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0; // variabler
    table = document.getElementById("display"); // henter tabellen fra html
    switching = true; // variabel som brukes til å skjekke om et bytte foretas

    dir = "asc"; // bestemmer retningen det skal sorteres i

    while (switching) { // løkke som kjøres så lege to rader skal byttes
        switching = false;
        rows = table.rows; // henter alle radene i tabellen
        for (i = 1; i < (rows.length - 1); i++) { // løkke som går gjennom alle radene i tabellen
            shouldSwitch = false;

            x = rows[i].getElementsByTagName("td")[n]; // henter cellen fra raden 'i' og kolonnen 'n' 
            y = rows[i + 1].getElementsByTagName("td")[n]; // henter cellen som ligger under cellen 'x' i tabellen

            if (dir == "asc") { // skjekker hvilken retning sorteringen skal skje
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) { // sammenligner verdiene i cellene 'x' og 'y'
                    shouldSwitch = true; // dersom x er større enn y betyr det at radene skal bytte
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    shouldSwitch = true; // dersom x er mindre enn y betyr det at radene skal bytte
                    break;
                }
            }
        }
        if (shouldSwitch) { // skjekker om et bytte skal foretas
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]); // bytter plass på de to radene
            switching = true; // setter switching = true så løkken kjøres igjen
            switchcount++; // teller antall bytter
        } else if (switchcount == 0 && dir == "asc") { // dersom ingen bytter er gjort og sortering er satt til 'asc' blir sorteringsretningen endret
            dir = "desc";
            switching = true;
        }
    }
}

function searchFunction() { // funksjon for filtrer tabellen med  det som står i søkefeltet
    var filter, row, cols, filtered, i, j; // variabler
    filter = document.getElementById("searchbar").value.toUpperCase(); // henter søkefeltet fra html og gjør det til uppercase
    row = document.getElementById("display").getElementsByTagName("tr"); // henter alle radene i tabelen fra html

    for (i = 1; i < row.length; i++) { // løkke som går gjennom alle radene i tabelen
        filtered = false; // variabel som brukes til å markere om en rad skal vises eller ikke
        cols = row[i].getElementsByTagName("td"); // henter alle kolonnene i en spesifikk rad
        for (j = 0; j < cols.length; j++) { // løkke som går gjennom alle kolonnene i raden
            if (cols[j]) { // dersom kolonnen ikke er tom kjører denne
                if (cols[j].innerHTML.toUpperCase().indexOf(filter) > -1) { // sjekker innholdet i cellen om det matcher med søket
                    filtered = true; // dersom det er en match blir filtered satt til true
                }
            }
        }
        if (filtered === true) { // dersom filtered er lik true blir raden vist
            row[i].style.display = '';
        }
        else { // dersom filtered er lik false blir raden ikke vist
            row[i].style.display = 'none';
        }
    }
}