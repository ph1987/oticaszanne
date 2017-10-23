/*==============================
	STYLE SWITCHER SCRIPT INSTALLATION
 ===============================*/
 
(function($) {
	"use strict";

	$(".color" ).click(function(){
		$("#colors").attr("href", "assets/css/color.css" );
		return false;
	});

	$(".color1" ).click(function(){
		$("#colors").attr("href", "assets/css/color1.css" );
		return false;
	});

	$(".color2" ).click(function(){
		$("#colors").attr("href", "assets/css/color2.css" );
		return false;
	});

	$(".color3" ).click(function(){
		$("#colors").attr("href", "assets/css/color3.css" );
		return false;
	});

	$(".color4" ).click(function(){
		$("#colors").attr("href", "assets/css/color4.css" );
		return false;
	});

	$(".color5" ).click(function(){
		$("#colors").attr("href", "assets/css/color5.css" );
		return false;
	});

	$(".color6" ).click(function(){
		$("#colors").attr("href", "assets/css/color6.css" );
		return false;
	});

	$(".color7" ).click(function(){
		$("#colors").attr("href", "assets/css/color7.css" );
		return false;
	});
	
	// Style Switcher	
	$('#style-switcher').animate({
		left: '-220px'
	});

	$('#style-switcher h2 a').click(function(e){
		e.preventDefault();
		var div = $('#style-switcher');
		console.log(div.css('left'));
		if (div.css('left') === '-220px') {
			$('#style-switcher').animate({
				left: '0px'
			}); 
		} else {
			$('#style-switcher').animate({
				left: '-220px'
			});
		}
	});

	$('.colors li a').click(function(e){
		e.preventDefault();
		$(this).parent().parent().find('a').removeClass('active');
		$(this).addClass('active');
	});
    
})(jQuery);