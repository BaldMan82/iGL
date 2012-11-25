
// Attributes
attribute mediump vec3 a_position;
attribute mediump vec3 a_normal;
attribute mediump vec2 a_uv;


// Structs
struct Light {	
	highp vec4 position; 
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;	
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform mediump vec2 u_centerPoint;

//uniform lowp vec4 u_eyePos;

uniform Light u_light;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying highp vec2 v_uv;

void calcLightning(mediump vec4 position, mediump vec4 normal);

void main() 
{
	highp vec4 position =  vec4(a_position.x, a_position.y, a_position.z, 1);		
	
	gl_Position = u_modelViewProjectionMatrix *position;

	mediump vec4 transformedPosition = u_modelViewMatrix*position;
	calcLightning(transformedPosition, vec4(a_normal.x, a_normal.y, a_normal.z, 1));
	
	v_uv = vec2((transformedPosition.x + 100.0) / 200.0 , (100.0 - transformedPosition.y) / 200.0);
}

void calcLightning(mediump vec4 position, mediump vec4 normal)
{	
	vec4 lightDir = u_light.position - position;
	mediump vec3 transformedNormal =  vec3(u_transposeAdjointModelViewMatrix * normal);
	vec3 lightVector = vec3(lightDir);

	v_ambientColor = vec4(0.2,0.2,0.2,1);

	mediump vec3 L = normalize(lightVector);
	transformedNormal = normalize(transformedNormal);

	highp float lambertTerm = dot(transformedNormal,L);
	
	v_diffuseColor = u_light.diffuse * clamp(lambertTerm, 0.0, 1.0);

}
