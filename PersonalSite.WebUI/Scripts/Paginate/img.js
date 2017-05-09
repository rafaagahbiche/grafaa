var readyToUpdate = 0;
var currentpage = 1;

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
}

$(document).ready(function () {
    //setHeightWidthSections();
});

$(window).load(function () {
    //setHeightWidthSections();
});

$(function () {
    var myObj = $('ul#pagination').paginatearticle({
        nextSelector: 'li.nav-next',
        prevSelector: 'li.nav-prev',
        pagerSelector: 'li.nav-numbers',
        itemslistSelector: 'div.img-list',
        listcontainerSelector: 'div.img-list-container',
        pagercontainerSelector: '.pager-container',
        currentpage: 1
    });

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
    //$('.img-display').each(function () {
    //    var img = $(this);
    //    centerChildren(img);
    //    positionningImageDisplay(img);
    //});

    //$("body").keydown(function (e) {
    //    if (e.keyCode == 37) { // left
    //        if (currentpage > 1) {
    //            navigateTo(currentpage - 1);
    //        }
    //    }
    //    else if (e.keyCode == 39) { // right
    //        if (currentpage < $('ul#pagination').children('li').length) {
    //            navigateTo(currentpage + 1);
    //        }
    //    }
    //});
});

