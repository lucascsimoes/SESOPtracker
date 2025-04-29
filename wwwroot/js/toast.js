export function toast(type, text) {
    $(".toast .toast-body").html("")

    if (type == "danger" || type == "success") {
        $(".toast .toast-body").prepend(`<svg class="${ type }" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-circle-check-big-icon lucide-circle-check-big"><path d="M21.801 10A10 10 0 1 1 17 3.335"/><path d="m9 11 3 3L22 4"/></svg>`)
    } else {
        $(".toast .toast-body").prepend('<svg class="success" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-circle-check-big-icon lucide-circle-check-big"><path d="M21.801 10A10 10 0 1 1 17 3.335"/><path d="m9 11 3 3L22 4"/></svg>')
    }
   
    $(".toast").addClass("show")
    $(".toast .toast-body").append(text)

    setTimeout(() => {
        $(".toast").removeClass("show")
    }, 4000)
}