$(document).ready(function () {
    //addBasket
    $(".addBasket").click(function (ev) {
        ev.preventDefault();
        let url = $(this).attr("href");
        fetch(url)
            .then(response => response.text())
            .then(data => {
                $(".basketModal").html(data);
            })
    })
})