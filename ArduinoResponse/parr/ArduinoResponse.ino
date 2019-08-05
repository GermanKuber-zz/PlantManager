bool value = true;
int delayValue = 100;
void setup() {
	Serial.begin(115200);
}

void loop() {
	String valor = Serial.readStringUntil('|');
	//Serial.println("Valor original LEido");
	//Serial.println(valor);
	String comando = "";
	String pin = "";
	String valorPin = "";
	bool leerComando, leerPin, leerValor = false;
	for (int i = 0; i <= valor.length(); i++) {
		if (valor.charAt(i) == ',') {
			leerComando = false;
		}
		if (valor.charAt(i) == 'C') {
			leerComando = true;
			leerPin = false;
			leerValor = false;
		}
		else 	if (valor.charAt(i) == 'P') {
			leerComando = false;
			leerPin = true;
			leerValor = false;
		}
		else 	if (valor.charAt(i) == 'V') {
			leerComando = false;
			leerPin = false;
			leerValor = true;
		}
		else {
			if (valor.charAt(i) != '=' && valor.charAt(i) != ',') {
				String newString = String(valor.charAt(i));
				if (leerComando) {
					comando.concat(newString);
				}
				else if (leerPin) {
					pin.concat(newString);
				}
				else if (leerValor) {

					valorPin.concat(newString);
				}
			}
		}
	}
	comando.replace("\n", "");
	pin.replace("\n", "");
	valorPin.replace("\n", "");


	bool isAnalog = false;
	bool isDigital = false;
	for (int i = 0; i <= 15; i++)
	{
		String port = "A";
		port.concat(i);
		if (pin.equalsIgnoreCase(port)) {
			isAnalog = true;
		}
	}
	if (!isAnalog) {
		for (int i = 0; i <= 53; i++)
		{
			String port = String(i);
			if (pin.equalsIgnoreCase(port)) {
				isDigital = true;
				isAnalog = false;
			}
		}
	}
	int valueRead = -1;
	
	if (isAnalog) {
		String local = String(pin);
		int pintInt = portConvert(pin);
		comando.replace("\r\n", "");
		comando.replace("\n", "");
		comando.replace(" ", "");
		if (comando.equalsIgnoreCase("R")) {
			pinMode(pintInt, INPUT);
			valueRead = analogRead(pintInt);
			sendData(valueRead, pin);
		}
		else 	if (comando.equalsIgnoreCase("W")) {
			pinMode(pintInt, OUTPUT);
			analogWrite(pintInt, valorPin.toInt());
			valueRead = analogRead(pintInt);
			sendData(valueRead, pin);
		}
	}
	else if (isDigital) {
	
		//Si es un PIN digital
		if (comando.equalsIgnoreCase("R")) {
		
			pinMode(pin.toInt(), INPUT);
			valueRead = digitalRead(pin.toInt());
			sendData(valueRead, pin);
		
		}
		else 	if (comando.equalsIgnoreCase("W")) {
			pinMode(pin.toInt(), OUTPUT);
			valorPin.replace("\n", "");		
			if (valorPin.equalsIgnoreCase("0")) {
				digitalWrite(pin.toInt(), HIGH);
			}
			else {
				digitalWrite(pin.toInt(), LOW);
			}
			valueRead = digitalRead(pin.toInt());
			if (valueRead == 0) {
				sendData(1, pin);
			}else if (valueRead == 1) {
				sendData(0, pin);
			}			
		}
	}
	delay(delayValue);

}
void sendData(int data, String pin) {	
	String localPin = String(pin);
	String localData = String(data);
	String sendData = String("P=" + localPin + ",V=" + localData + "|");
	sendData.replace("\n", "");
	Serial.println(sendData);
}
int portConvert(String pin) {
	int pintInt = 0;
	if (pin.equalsIgnoreCase("A0")) {
		pintInt = A0;
	}
	if (pin.equalsIgnoreCase("A1")) {
		pintInt = A1;
	}
	if (pin.equalsIgnoreCase("A2")) {
		pintInt = A2;
	}
	if (pin.equalsIgnoreCase("A3")) {
		pintInt = A3;
	}
	if (pin.equalsIgnoreCase("A4")) {
		pintInt = A4;
	}
	if (pin.equalsIgnoreCase("A5")) {
		pintInt = A5;
	}
	if (pin.equalsIgnoreCase("A6")) {
		pintInt = A6;
	}
	if (pin.equalsIgnoreCase("A7")) {
		pintInt = A7;
	}
	if (pin.equalsIgnoreCase("A8")) {
		pintInt = A8;
	}
	if (pin.equalsIgnoreCase("A9")) {
		pintInt = A9;
	}
	if (pin.equalsIgnoreCase("A10")) {
		pintInt = A10;
	}
	if (pin.equalsIgnoreCase("A11")) {
		pintInt = A11;
	}
	if (pin.equalsIgnoreCase("A12")) {
		pintInt = A12;
	}
	if (pin.equalsIgnoreCase("A13")) {
		pintInt = A13;
	}
	if (pin.equalsIgnoreCase("A14")) {
		pintInt = A14;
	}
	if (pin.equalsIgnoreCase("A15")) {
		pintInt = A15;
	}
	return pintInt;
}