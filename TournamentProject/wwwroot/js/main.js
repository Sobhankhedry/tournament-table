$(window).scroll(function(){
    let position = $(this).scrollTop();
    if(position >= 200 ){
        $('.nav-menu').addClass('custom-navbar')
    }else{
        $('.nav-menu').removeClass('custom-navbar')
    }
})


$(document).ready(function () {
    $('form').on('submit', function () {
        $('.input-validation-error').css('border-color', 'red');
    });
});

//$(window).scroll(function () {
//    let position = $(this).scrollTop();
//    if (position >= 200) {
//        $('.nav-menu').addClass('custom-navbar')
//    } else {
//        $('.nav-menu').removeClass('custom-navbar')
//    }
//}) 