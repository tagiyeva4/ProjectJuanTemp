$(document).ready(function () {
    //addBasket
    $(".addBasket").click(function (ev) {
        ev.preventDefault();
        let url = $(this).attr("href");
        fetch(url)
            .then(response => response.text())
            .then(data => {
                $(".minicart-content-box").html(data);
            })
    });

    
    $('.ion-android-close').on('click', function (e) {
        var dom = $('.off-canvas-wrapper').children();
        e.preventDefault();
        var $this = $(this);
        $this.parents('.open').removeClass('open');
        dom.find('.global-overlay').removeClass('overlay-open');
    });


    //without refresh count increase

    //const addToBasketButtons = document.querySelectorAll('.addToBasketBtn');
    //const basketModalArea = document.querySelector("#miniCart");
    //const basketModalCount = document.querySelector(".notification")
    //function RenderRemoveButtonEvent() {
    //    $('.button-close').on('click', function (e) {
    //        var dom = $('.main-wrapper').children();
    //        e.preventDefault();
    //        var $this = $(this);
    //        $this.parents('.open').removeClass('open');
    //        dom.find('.global-overlay').removeClass('overlay-open');
    //    });
    //}
    //addToBasketButtons.forEach(btn => {
    //    btn.addEventListener('click', async (e) => {
    //        e.preventDefault();

    //        const response = await fetch(btn.href);

    //        const result = await response.text();

    //        basketModalArea.innerHTML = result;

    //        RenderRemoveButtonEvent();

    //        Swal.fire({
    //            position: "center",
    //            icon: "success",
    //            showConfirmButton: false,
    //            timer: 1500
    //        });

    //        let count = basketModalArea.querySelectorAll(".minicart-product").length

    //        document.querySelector(".notification").innerHTML = count
    //        document.querySelector(".basketModalCount2").innerHTML = count
    //    })
    //});


    //searchInput

    $("#searchInput").on("keyup", function () {


        console.log("salam")
        let value = $(this).val();
        if (value)
        {
            fetch("/home/search?search=" + value)
                .then(response => response.text())
                .then(data => {
                    $(".searchList").html(data);
                })
        }
    });

})
