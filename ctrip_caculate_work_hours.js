//inject jQuery
(function(){
    if(typeof jQuery === 'undefined'){                
        var script = document.createElement("script");
        script.src = "http://libs.baidu.com/jquery/1.9.0/jquery.min.js"; //using baidu cdn. maybe using self cdn        
        document.getElementsByTagName('head')[0].appendChild(script);
    }
})();

function caculateWorkHours(){
	// if(typeof jQuery === 'undefined'){
	// 	//inject jQuery
	// 	var script = document.createElement("script");
	// 	script.src = "http://libs.baidu.com/jquery/1.9.0/jquery.min.js"; //using baidu cdn. maybe using self cdn
	// 	if(script.readyState){
	// 	   	var runned = 0;
	// 		script.onreadystatechange = function(){
	// 			if(script.readyState == "loaded" || script.readyState == "complete"){
	// 				if(!runned){
	// 					runned = 1;
	// 					ShowWorkHours();
	// 				}
	// 			}
	// 		};
	// 	}else{
	// 		script.onload = ShowWorkHours;				
    // 	}
    //        document.getElementsByTagName('head')[0].appendChild(script);
    // }else{
    //        ShowWorkHours();    
    // }

    //Caculate work hours, work days
	function caculateWorkHoursAndWorkDays (){
		var target = [];
        // var temp = $('td[align=center]').toArray();
        var temp = $(window.frames['Main'].document).find('td[align=center]').toArray();
        for(var i = 0; i < temp.length; i++){
            var item = temp[i];
            var text = item.innerText;
            var html = item.innerHTML;
            if(text.indexOf('~')>0 && html.indexOf('table')<0)
                target.push(text);  
        }
		var totalWorkMinutes = 0;
		target.forEach(function(x){
			totalWorkMinutes += caculateTimeSpan(x);
		});

		function caculateTimeSpan(cardTimePair){
			var startTimeAndEndTimePair = cardTimePair.split('~'); //TODO: Check reuslt array length equal 2
			var startDateTime = buildDateTime(startTimeAndEndTimePair[0]);
			var endDateTime = buildDateTime(startTimeAndEndTimePair[1]);
			var millisecondsDiff  = endDateTime - startDateTime;
			var minutesDiff = Math.floor(millisecondsDiff / 1000 / 60);
			return minutesDiff;
		}
		function buildDateTime(caculateDateTimeSource){		
			var cacaulateDateTime = new Date();
			var endTime = new Date();		
			var HourAndMinutePair = caculateDateTimeSource.split(':');//TODO: Check result array length equal 2
			cacaulateDateTime.setHours(HourAndMinutePair[0]);
			cacaulateDateTime.setMinutes(HourAndMinutePair[1]);
			return cacaulateDateTime;
		}
		return {
			workHours : totalWorkMinutes/60,
			workDays  : target.length
		}
	};

	//current work hours
	//already work hours
	//average work hours per day        
	return (function ShowWorkHours(totalWorkHours){
        var temp = caculateWorkHoursAndWorkDays();
        var workHorus = "当月你工作了 " + temp.workHours + " 小时。";
        var workDays = "当月你工作了 " + temp.workDays +  "天。"
        var averageHours = "当月你每天的工时为：" + temp.workHours/temp.workDays;
        alert(workHorus + "\n" + workDays + "\n" + averageHours);        
    })();
};

if(typeof jQuery === 'undefined'){                
     setTimeout( caculateWorkHours , 5000);
}else{
     caculateWorkHours();
}
