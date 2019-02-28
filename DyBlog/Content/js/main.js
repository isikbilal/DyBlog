"use strict"

$(document).ready(function(){

/*---------------------------------------*/
/* Sticky Header Nav                     */
/*---------------------------------------*/

     var nav = $('.sticky-menu');
     var body = $('body');
     // var headerheight = 70;
    
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 100) {
            nav.addClass("fixed");
        } else {
            nav.removeClass("fixed");
        }
    });

/*---------------------------------------*/
/* Mobile Only Slide Menu                */
/*---------------------------------------*/

$(".sticky-menu i, .toggle-menu i, .slide-menu .close i").on('click', function() {
        $(".mobile-navigation").toggleClass("show");
        $("html").toggleClass("inactive");

        // Check for an active search form and close it
        if($('.search-form').hasClass('show-search')) {
           $(".search-form").slideToggle(300);
           $(".search-form").toggleClass("show-search");
        }
});

// Expand the parent/child menu
$('.slide-menu li.menu-item-has-children > a').on('click', function(event){
    event.preventDefault();
   $(this).next('.sub-menu').slideToggle(200);
   $(this).toggleClass("close");
});

/*---------------------------------------*/
/* Search drop down                      */
/*---------------------------------------*/

  $("li.search i, i.toggle").on('click', function() {
        $(".search-form").slideToggle(300);
        $(".search-form").toggleClass("show-search");

        // Focus the cursor on the search field when it's visible
        $(".search-form.show-search .search-field").focus();

        // Remove focus when not visible
        $('.search-form:not(.show-search) .search-field').blur();

        // Check for an active slide menu and close it
        if($('.mobile-navigation').hasClass('show')) {
           $(".mobile-navigation").toggleClass("show");
        }
});

/*---------------------------------------*/
/* ESC key close of open toggle elements */
/*---------------------------------------*/

$(document).keyup(function(e) { 
        if (e.keyCode == 27) { // esc keycode
            if($('.search-form').hasClass('show-search')) {
               $(".search-form").slideToggle(300);
               $(".search-form").toggleClass("show-search");
            }
            if($('.mobile-navigation').hasClass('show')) {
               $(".mobile-navigation").toggleClass("show");
            }
        }
    });

/*---------------------------------------*/
/* Wp Instagram Widget                   */
/*---------------------------------------*/

// Add .cols class equal to the number of items for each instance of the widget
$(".instagram-footer ul.instagram-pics").each(function(){
var cols = $("li", this).length;
// If we have more than 6 we'll divide the number so we can create two rows of pics
if (cols > 6 ) {
    cols = cols / 2;
}
$(this).addClass("pics"+ cols +"");

});

});

