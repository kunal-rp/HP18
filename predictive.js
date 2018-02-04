
const NO_EVENTS = 5;
const _events = [
				  {name: "cafe", value: 0, percentage: 0}, 
				  {name: "school", value: 0, percentage: 0},
				  {name: "landmark", value: 0, percentage: 0},
				  {name: "farm", value: 0, percentage: 0},
				  {name: "store", value: 0, percentage: 0}
				];


function stock_flux(){
	//step through all possible events to determine if change occurs
	for (var index = 0; index < NO_EVENTS; index++){
		var _change =  yes_or_no ();
		if (_change) {
			//if change occurs use sigmoid function to randomly determine percentage value
            _events[index].percentage = sigmoid(randomInRange());
			var positive = yes_or_no ();
			//if change occurs change value to reflect positive or negative change ie +1 or -1
			if (positive){
				_events[index].value = 1;
			}
			else
				_events[index].value = -1;	
		}
	}
}

//used to determinage 
function randomInRange() {
  return Math.random() * (6.0 + 6.0) -6.0;
}

function sigmoid(t) {
    return 1/(1+Math.pow(Math.E, -t));
}

function yes_or_no (){
	var coin = Math.floor(Math.random() * Math.floor(10000));

		if (coin % 2 === 0)
			return true;
		else
			return false;
}




//tester = sigmoid(randomInRange());
//console.log(tester);