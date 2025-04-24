export function toast(type, text) {
    $(".toast").removeClass("text-bg-success")
    $(".toast").removeClass("text-bg-danger")

    if (type == "danger" || type == "success") {
        $(".toast").addClass(`text-bg-${ type }`)
    } else {
        $(".toast").addClass("text-bg-success")
    }
   
    $(".toast").addClass("show")
    $(".toast .toast-body").text(text)

    setTimeout(() => {
        $(".toast").removeClass("show")
    }, 4000)
}