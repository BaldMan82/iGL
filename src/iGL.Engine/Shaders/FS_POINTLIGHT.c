

precision mediump float;

// Structs
struct Light {	
	vec4 position; 
	vec4 ambient;
	vec4 diffuse;
	vec4 specular;	
};

struct Material {
	vec4 ambient;
	vec4 diffuse;
	vec4 specular;
	float shininess;
};

// Uniforms
uniform Light u_light;
uniform Material u_material;

// Varyings
varying vec4 v_ambientColor;
varying vec3 v_normal;
varying vec3 v_eyepos;
varying vec3 v_lightVector;

void calcLightning(out vec4 color);

void main() 
{	
	vec4 color;
	calcLightning(color);

	gl_FragColor = color;
}

void calcLightning(out vec4 color)
{
	color = v_ambientColor;

	vec3 N = normalize(v_normal);
	vec3 L = normalize(v_lightVector);

	float lambertTerm = dot(N,L);
	
	if(lambertTerm > 0.0)
	{
		color += u_light.diffuse * 
		               u_material.diffuse * 
					   lambertTerm;	
		
		vec3 E = normalize(v_eyepos);
		vec3 R = reflect(-L, N);
		float specular = pow( max(dot(R, E), 0.0), 
		                 u_material.shininess );
		color += u_light.specular * 
		               u_material.specular * 
					   specular;	
	}	
}
