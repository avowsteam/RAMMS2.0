$(document).ready(function () {
    $('#MIR').addClass('activeMap');
    $('#info-box').css('display', 'block');
    $('#info-box').html($('#MIR').data('info'));
});

$(".path, .grp-path").hover(function (e) {
    $('#MIR').removeClass('activeMap');
  $('#info-box').css('display','block');
  $('#info-box').html($(this).data('info'));
});

$(".path, .grp-path").mouseleave(function (e) {
    $('#MIR').addClass('activeMap');
    $('#info-box').html($('#MIR').data('info'));
  //$('#info-box').css('display','none');
});

$(document).mousemove(function (e) {
  $('#info-box').css('top',e.pageY-$('#info-box').height()-30);
  $('#info-box').css('left',e.pageX-($('#info-box').width())/2);
}).mouseover();

var ios = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
if(ios) {
  $('a').on('click touchend', function() {
    var link = $(this).attr('href');
    window.open(link,'_blank');
    return false;
  });
}
