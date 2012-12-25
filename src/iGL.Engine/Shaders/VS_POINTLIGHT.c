
// Attributes
attribute highp vec3 a_position;
attribute highp vec3 a_normal;
attribute highp vec2 a_uv;


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

varying lowp vec4 v_color;
varying highp vec2 v_uv;

void calcLightning(mediump vec4 position, mediump vec4 normal);

void main() 
{
	highp vec4 position =  vec4(a_position.x / u_shortFloatFactor, a_position.y / u_shortFloatFactor, a_position.z / u_shortFloatFactor, 1);

	v_uv = vec2(a_uv.x / u_shortFloatFactor, a_uv.y / u_shortFloatFactor);

	calcLightning(position, vec4(a_normal.x / u_shortFloatFactor, a_normal.y / u_shortFloatFactor, a_normal.z / u_shortFloatFactor, 1));
	
	gl_Position = u_modelViewProjectionMatrix *position;
}

void calcLightning(highp vec4 position, highp vec4 normal)
{	
	highp vec4 lightDir = u_light.position - u_modelViewMatrix*position;
	highp vec4 transformedNormal = u_transposeAdjointModelViewMatrix*normal;
		
	lightDir = normalize(lightDir);
	transformedNormal = normalize(transformedNormal);

	highp float lambertTerm = clamp(dot(transformedNormal, lightDir)*1.5, 0.0, 1.0);
	
	v_color = u_light.diffuse * u_material.diffuse*lambertTerm +  u_material.ambient*u_globalAmbientColor + u_material.ambient*u_light.ambient;
	
}
