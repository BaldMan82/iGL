
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

struct Material {
	lowp vec4 ambient;
	lowp vec4 diffuse;
	lowp vec4 specular;
	lowp float shininess;
};

// Uniforms

uniform mat4 u_modelViewMatrix;
uniform mat4 u_modelViewProjectionMatrix;
uniform mat4 u_transposeAdjointModelViewMatrix;
uniform mediump vec2 u_textureScale;
uniform int u_blackBorder;

//uniform lowp vec4 u_eyePos;

uniform Light u_light;
uniform Material u_material;

uniform highp float u_shortFloatFactor;

uniform lowp vec4 u_globalAmbientColor;

varying lowp vec4 v_ambientColor;
varying lowp vec4 v_diffuseColor;
varying highp vec2 v_uv;

void calcLightning(mediump vec4 position, mediump vec4 normal);

void main() 
{
	highp vec4 position =  vec4(a_position.x / u_shortFloatFactor, a_position.y / u_shortFloatFactor, a_position.z / u_shortFloatFactor, 1);

	v_uv = vec2(a_uv.x / u_shortFloatFactor, a_uv.y / u_shortFloatFactor);

	calcLightning(position, vec4(a_normal.x / u_shortFloatFactor, a_normal.y / u_shortFloatFactor, a_normal.z / u_shortFloatFactor, 1));
	
	gl_Position = u_modelViewProjectionMatrix *position;
}

void calcLightning(mediump vec4 position, mediump vec4 normal)
{	
	vec4 lightDir = u_light.position - u_modelViewMatrix*position;
	mediump vec3 transformedNormal =  vec3(u_transposeAdjointModelViewMatrix * normal);
	vec3 lightVector = vec3(lightDir);

	v_ambientColor = u_material.ambient*u_globalAmbientColor  + u_material.ambient*u_light.ambient;

	mediump vec3 L = normalize(lightVector);
	transformedNormal = normalize(transformedNormal);

	highp float lambertTerm = dot(transformedNormal,L);
	
	v_diffuseColor = u_light.diffuse * u_material.diffuse  * clamp(lambertTerm, 0.0, 1.0);
	
	/*lambertTerm = 0.1;
	if(lambertTerm > 0.0)
	{		
		v_diffuseColor = u_light.diffuse * 
		               u_material.diffuse * 
					   lambertTerm;	
					   		   
	}*/
}
