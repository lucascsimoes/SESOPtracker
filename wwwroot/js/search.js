export function countRows() {
    const accordion = document.querySelectorAll(".accordion-item")
    if (accordion == null) {
        return;
    }

    Array.from(accordion).forEach(item => {
        const rows = item.querySelectorAll("tbody tr")
        const count = Array.from(rows).filter(item => window.getComputedStyle(item).display == "table-row").length
        item.querySelector(".count-rows").textContent = count

        if (count == 0) {
            item.querySelector("table").style.display = "none"
        } else {
            item.querySelector("table").style.display = "table"
        }
    })
}

export function checkMatch(element, text) {
    let matches = 0
    element.querySelectorAll("td").forEach((value) => {
        if (value.innerText.toLowerCase().trim().includes(text.toLowerCase().trim())) {
            matches++
        }
    })

    if (matches > 0) {
        element.style.display = "table-row"
    } else {
        element.style.display = "none"
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const input = document.querySelectorAll(".searchbar fieldset input")
    let tableRows;

    if (input.length != 0) {
        input.forEach(currentInput => {
            currentInput.onkeyup = () => {
                if (currentInput.closest(".modal") != null) {
                    tableRows = currentInput.closest(".modal").querySelectorAll("table tbody tr")
                } else {
                    tableRows = currentInput.closest("#container").querySelectorAll("table tbody tr")
                }

                tableRows.forEach((element) => checkMatch(element, currentInput.value))

                const quantity = Array.from(tableRows).filter(item => window.getComputedStyle(item).display == "table-row").length

                if (document.getElementById("quantityResult") != null) {
                    document.getElementById("quantityResult").innerHTML = quantity
                }

                countRows()
            }
        })
        
    }
})