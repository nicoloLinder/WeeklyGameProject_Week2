if(document.documentElement.clientHeight > document.documentElement.clientWidth){
    document.getElementById("unityContainer").style.width = "100vw";
    document.getElementById("unityContainer").style.height = "100vw";
    document.getElementById("container").style.width = "100vw";
    document.getElementById("container").style.height = "100vw";
    var percentage = (document.documentElement.clientWidth/document.documentElement.clientHeight *100)/2;
    document.getElementById("container").style.margin =  percentage +"% 0 0 0";
}else{
     document.getElementById("unityContainer").style.width = "100vh";
     document.getElementById("unityContainer").style.height = "100vh";
    document.getElementById("container").style.width = "100vh";
     document.getElementById("container").style.height = "100vh";
}


