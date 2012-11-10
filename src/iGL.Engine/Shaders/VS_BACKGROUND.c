
// Attributes
attribute highp vec3 a_position;
attribute highp vec3 a_normal;
attribute highp vec2 a_uv;


// Structs
struct Light {	
	lowp vec4 position; 
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;	
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;

//uniform lowp vec4 u_eyePos;

uniform Light u_light;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying highp vec4 v_position;

void calcLightning(mediump vec4 position, mediump vec4 normal);

void main() 
{
	highp vec4 position =  vec4(a_position.x, a_position.y, a_position.z, 1);		

	calcLightning(position, vec4(a_normal.x, a_normal.y, a_normal.z, 1));

	gl_Position = u_modelViewProjectionMatrix *position;
}

void calcLightning(mediump vec4 position, mediump vec4 normal)
{	
	v_position = u_modelViewMatrix*position;
	vec4 lightDir = u_light.position - v_position;
	mediump vec3 transformedNormal =  vec3(u_transposeAdjointModelViewMatrix * normal);
	vec3 lightVector = vec3(lightDir);

	v_ambientColor = vec4(0,0,0,1);

	mediump vec3 L = normalize(lightVector);
	transformedNormal = normalize(transformedNormal);

	lowp float lambertTerm = dot(transformedNormal,L);
	
	v_diffuseColor = u_light.diffuse * clamp(lambertTerm, 0.0, 1.0);

}
