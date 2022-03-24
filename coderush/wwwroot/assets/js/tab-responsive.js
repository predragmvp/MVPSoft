// Set dynamic tab width for responsiveness
$(function(){
    function setTabWidth(){
        if($(window).width() < 991){
            $(".tabs-container").each(function(index){
                var tabContainerWidth = 15;
                $(this).children().each(function(){
                    tabContainerWidth += $(this).outerWidth();
                })
                $(this).innerWidth(tabContainerWidth);
            });
        }
        else{
            $(".tabs-container").removeAttr("style");
        }
    }
    setTabWidth()

    $(window).resize(function(){
        setTabWidth();
    })
})