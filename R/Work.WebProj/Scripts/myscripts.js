$(document).ready(function () {
  //add class or id for BS components
  $("#sidebar .menu").attr("id","accordion");
  $("#sidebar .menu > li").addClass("panel");
  $("#sidebar .submenu").addClass("collapse");
  
  //add data attributes for BS components
  $("nav .toggle").attr({
    "data-toggle":"collapse",
    "data-target":".nav-bar"
  });
  $(".dropdown > a").attr("data-toggle","dropdown");
  $("#sidebar .menu a[href*='#menu']").attr({
    "data-toggle":"collapse",
    "data-parent":"#accordion"
  });
});