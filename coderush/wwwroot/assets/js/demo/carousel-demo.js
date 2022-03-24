// Basic Carousel
$('.basic').owlCarousel({
    loop: true,
    margin: 10,
    nav: true,
    autoplay: true,
    autoplayTimeout: 5500,
    navText: ["<i class='mdi mdi-chevron-left'></i>", "<i class='mdi mdi-chevron-right'></i>"],
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 1
        },
        1000: {
            items: 1
        }
    }
});

// Carousel with loop
$('.loop').owlCarousel({
    center: true,
    items: 2,
    loop: true,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 8500,
    nav: false,
    responsive: {
        600: {
            items: 4
        }
    }
});

// Carousel without loop
$('.noloop').owlCarousel({
    items: 5,
    loop: false,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 6000,
    responsive: {
        0:{
            items: 2
        },
        600: {
            items: 4
        }
    }
});

// Carousel with right direction
$('.rtl').owlCarousel({
    rtl: true,
    loop: true,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 3000,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 3
        },
        1000: {
            items: 4
        }
    }
});