document.addEventListener('DOMContentLoaded', function () {
    const selectList = document.querySelectorAll('form select')

    selectList.forEach(select => {
        select.addEventListener('change', function () {
            select.value = this.value

            Array.from(select.children).forEach(item => {
                if (item.value != select.value) {
                    item.removeAttribute("selected")
                } else {
                    item.setAttribute("selected", "selected")
                }
            })

            if (select.nextElementSibling != null) {
                select.nextElementSibling.value = this.value
            }
        })
    })
})