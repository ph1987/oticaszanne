/*
 *****************************************************
 *	CUSTOM JS DOCUMENT                              *
 *	Single window load event                        *
 *   "use strict" mode on                           *
 *****************************************************
 */
$(window).on('load', function() {

    "use strict";

    var preLoader = $('.loading');
    var fancybox = $('.fancybox');
    var comingsoontimer = $("#coming-soon-timer");
    var faqs = $('#faq');
    var rangslider = $("#rangslider");
    var showlogin = $('.showlogin');
    var loginDiv = $('.login');
    var showcoupon = $('.showcoupon');
    var checkout_coupon = $('.checkout_coupon');
    var differentAddress = $('#ship-to-different-address-checkbox');
    var shippingFields = $('.shipping-fields');
    var createAccountCheck = $('#createaccount');
    var createAccount = $('.create-account');
    var vSlider = $('.slider8');
    var vSlider1 = $('.slider10');
    var hSlider = $('.slider9');
    var tabLinks = $('.tablinks');

    /*
    ========================================
    PreLoader On window Load
    ========================================
    */
    if (preLoader.length) {
        preLoader.fadeOut();
    }

    /*
    ========================================
    Fancybox Setting
    ========================================
    */

    if (fancybox.length) {
        fancybox.fancybox();
    }

    /*
    ========================================
    Accordion functions Calling
    ========================================
    */

    if (faqs.length) {
        faqs.accordion();
    }


    /*
    ========================================
    Coming Soon Setting
    ========================================
    */

    if (comingsoontimer.length) {
        jewellaryCounterint();
    }

    /*
    ========================================
    Price Filter Setting
    ========================================
    */
    if (rangslider.length) {
        rangSliderint();
    }

    /*
    ========================================
    CheckOut Page Setting
    ========================================
    */


    showlogin.on('click', function(e) {
        e.preventDefault();
        loginDiv.slideToggle("slow");
    });

    showcoupon.on('click', function(e) {
        e.preventDefault();
        checkout_coupon.slideToggle("slow");
    });

    differentAddress.on('change', function() {
        if (this.checked) {
            shippingFields.slideToggle('slow');
        } else {
            shippingFields.slideToggle('slow');
        }
    });

    createAccountCheck.on('change', function() {
        if (this.checked) {
            createAccount.slideToggle('slow');
        }
    });

    /*
    ========================================
    vertical slider
    ========================================
    */
    vSlider.bxSlider({
        mode: 'vertical',
        slideWidth: 300,
        minSlides: 3,
        slideMargin: 10,
        pager: false,
    });   

	vSlider1.bxSlider({
		slideWidth: 200,
		minSlides: 4,
		maxSlides: 3,
		pager: false,
		slideMargin: 10,

	});
  
	hSlider.bxSlider({
        slideWidth: 300,
        minSlides: 3,
        slideMargin: 10,
        pager: false,
    });


    //========================================
    // Tabs Settings
    //======================================== 	

    tabLinks.on('click', function() {
        var dataId = $(this).attr('data-id');
        tabCustom(event, dataId);
    });


    /*
========================================
Product Grig list Setting
========================================
*/

    $('.viewGrid').on('click', function() {

        document.getElementById('productsgriads').style.display = 'block';
        document.getElementById('productslist').style.display = 'none';
    });

    $('.viewList').on('click', function() {

        document.getElementById('productsgriads').style.display = 'none';
        document.getElementById('productslist').style.display = 'block';
    });


    /*
    ========================================
    Owl Carousel Setting
    ========================================
    */

    owlCarouselInit();


});


//========================================
// Owl Carousel functions
//======================================== 	

function owlCarouselInit() {

    "use strict";

    var mainSlider = $('#main-slider');
    var Partner = $('#Partner-slider');
    var productslider = $('#product-items-slider');
    var productsidebarslider = $('#product-sidebar-slider');
    var nextNav = '<i class="fa fa-long-arrow-right" aria-hidden="true"></i>';
    var prevNav = '<i class="fa fa-long-arrow-left" aria-hidden="true"></i>';


    mainSlider.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        dots: false,
        
        navText: [prevNav, nextNav],
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


    Partner.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        dots: false,
        navText: [prevNav, nextNav],
        autoplay: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
    });


    productslider.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        autoplay: true,
        navText: [prevNav, nextNav],
        dots: false,
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

    productsidebarslider.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        autoplay: true,
        navText: [prevNav, nextNav],
        dots: false,
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
}




/*
========================================
Filter
========================================
*/
function rangSliderint() {

    "use strict";

    var sliderRange = $("#slider-range");
    var amounts = $("#amount");

    sliderRange.slider({
        range: true,
        min: 0,
        max: 500,
        values: [75, 300],
        slide: function(event, ui) {
            amounts.val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    amounts.val("$" + sliderRange.slider("values", 0) +
        " - $" + sliderRange.slider("values", 1));

}
/*
========================================
Timer
========================================
*/
// Set the date we're counting down to
function jewellaryCounterint() {

    "use strict";

    var countDownDate = new Date("dec 24, 2017 15:37:25").getTime();

    // Update the count down every 1 second
    var x = setInterval(function() {

        // Get todays date and time
        var now = new Date().getTime();

        // Find the distance between now an the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);

        // Output the result in an element with id="demo"
        document.getElementById("days").innerHTML = days;

        document.getElementById("hours").innerHTML = hours;

        document.getElementById("minutes").innerHTML = minutes;

        document.getElementById("seconds").innerHTML = seconds;


    }, 1000);
}



/*
 *****************************************************
 *	END OF THE JS 									*
 *	DOCUMENT                       					*
 *****************************************************
 */

function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

//========================================
// Tabs function settings
//======================================== 
function tabCustom(evt, dataId) {

    "use strict";

    var i, tabcontent, tablinks;

    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(dataId).style.display = "block";
    evt.currentTarget.className += " active";


}