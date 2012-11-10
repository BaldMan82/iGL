

struct Light {	
	lowp vec4 position; 
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;	
};

// Uniforms
uniform Light u_light;
uniform mediump vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying highp vec4 v_position;

void main() 
{		
	float x1 = v_position.x;
	float x2 = 0;
	float y1 = v_position.y + 10;
	float y2 = 1;

	float angle = mod(atan( (x1*y2) - (x2*y1), (x1 * x2) + (y1 * y2)),(2 * 3.1415));

	if (mod(floor(angle / 0.2), 2) == 0){					 
		gl_FragColor = v_ambientColor*vec4(0.525,0.623,0.886,1) + v_diffuseColor*vec4(0.525,0.623,0.886,1);
	}
	else {
		gl_FragColor = v_ambientColor*vec4(0.478,0.588,0.874,1) + v_diffuseColor*vec4(0.478,0.588,0.874,1);
	}
		
}
