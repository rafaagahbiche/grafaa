var readyToUpdate = 0;
var currentpage = 1;

function slidePages(nextpage) {
    var slidepx = $(".img-list").width();
    if (currentpage > nextpage) {
        // slide to right
        $('ul#pagination').animate({ marginLeft: "+=" + (currentpage - nextpage) * slidepx }, { duration: 'slow' });
    }
    else {
        if (currentpage < nextpage) {
            // slide to left
            $('ul#pagination').animate({ marginLeft: "-=" + (nextpage - currentpage) * slidepx }, { duration: 'slow' });
        }
    }
}

/**
*   Tools Function
*/

function centerBoxInsideParent(box, parent) {
    var boxPositionTop = (parent.outerHeight() / 2) - (box.outerHeight() / 2);
    var boxPositionLeft = (parent.outerWidth() / 2) - (box.outerWidth() / 2);
    box.css({
        'top': boxPositionTop + 'px',
        'left': boxPositionLeft + 'px'
    });
}

function centerChildren(imgDisplay) {
    var delDiv = imgDisplay.children('.del-background');
    var delConfirm = imgDisplay.children('.del-confirm');
    var loading = imgDisplay.children('.loading');

    var detailsDiv = imgDisplay.children('.img-details');
    var detailsModifyDiv = imgDisplay.children('.img-details-modify');

    delDiv.height(detailsDiv.height() + imgDisplay.height());
    //    centerBoxInsideParent(delConfirm, delDiv);
    centerBoxInsideParent(delConfirm, imgDisplay);

    //loading.height(detailsModifyDiv.height() +imgDisplay.height());
    centerBoxInsideParent(loading, imgDisplay);
}

function positionningImageDisplay(img) {
    var imgContainer = $(img).children('.img-container');
    var imgDetails = $(img).children('.img-details');
    var imgDetailsEdition = $(img).children('.img-details-modify');
    var x = parseInt(imgContainer.css('left'), 10);
    var leftPositionInt = parseInt(imgContainer.position().left, 10) - parseInt(imgContainer.css('border-left-width'), 10)
    imgDetails.css('left', leftPositionInt + 'px');
    imgDetailsEdition.css('left', leftPositionInt + 'px');
}

function setHeightWidthSections() {
    var maxHeight = 0;
    // var liWidth = $(window).width()*0.65;
    var liWidth = $('div.img-list').width();
    $('ul#pagination').children('li').each(function () {
        $(this).css('width', liWidth + 'px');
        if ($(this).outerHeight(false) > maxHeight) {
            maxHeight = $(this).outerHeight(false);
        }
    });

    $('div.img-list').css('height', maxHeight + 10 + 'px');
    $('div.img-list-container').css('height', maxHeight + 10 + 'px');
    $('.pager-container').css('width', liWidth + 'px');

    var ulWidth = $('ul#pagination').children('li').length * liWidth;
    $('ul#pagination').css('width', ulWidth + 'px');
}

$(document).ready(function () {
    setHeightWidthSections();
});

$(window).load(function () {
    setHeightWidthSections();
});

$(function () {
    if (isMobile) {
        $('ul#pagination').on('swipeleft', function () {
            if (currentpage < $('ul#pagination').children('li').length) {
                navigateTo(currentpage + 1);
            }
        });
        $('ul#pagination').on('swiperight', function () {
            if (currentpage > 1) {
                navigateTo(currentpage - 1);
            }
        });
    }
    $('.img-display').each(function () {
        var img = $(this);
        centerChildren(img);
        positionningImageDisplay(img);
    });

    $("body").keydown(function (e) {
        if (e.keyCode == 37) { // left
            if (currentpage > 1) {
                navigateTo(currentpage - 1);
            }
        }
        else if (e.keyCode == 39) { // right
            if (currentpage < $('ul#pagination').children('li').length) {
                navigateTo(currentpage + 1);
            }
        }
    });

    function navigateTo(page) {
        $('._current', '#pagination').removeClass('_current');
        $('#p' + page).addClass('_current');
        slidePages(page);
        currentpage = page;
        $('li.nav-numbers').children('span').first().html(currentpage);
    }

    $('li.nav-next').on('click', function () {
        if (currentpage < $('ul#pagination').children('li').length) {
            navigateTo(currentpage + 1);
        }
        else {
            $(this).css('color', 'black');
        }
    })

    $('li.nav-prev').on('click', function () {
        if (currentpage > 1) {
            navigateTo(currentpage - 1);
        }
        else {
            $(this).css('color', 'black');
        }
    })
});

