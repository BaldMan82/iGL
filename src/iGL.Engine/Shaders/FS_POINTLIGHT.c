

// Structs
struct Light {	
	lowp vec4 position; 
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
uniform Light u_light;
uniform Material u_material;
uniform lowp vec4 u_globalAmbientColor;

uniform float u_hasTexture;
uniform float u_hasNormalTexture;

// Varyings
varying lowp vec4 v_ambientColor;
varying lowp vec3 v_normal;
varying lowp vec3 v_eyepos;
varying lowp vec3 v_lightVector;
varying lowp vec2 v_uv;

uniform sampler2D s_texture;
uniform sampler2D s_normalTexture;

void calcLightning(out mediump vec4 color, mediump vec4 textureColor, lowp vec3 normal, lowp vec3 lightVector);

void main() 
{	
	mediump vec4 color;
	mediump vec4 textureColor = texture2D(s_texture, v_uv);
	
	lowp vec3 normal = v_normal;
	lowp vec3 lightVector = v_lightVector;

	if (u_hasNormalTexture > 0 )
	{
		lowp vec4 normColor = texture2D (s_normalTexture, v_uv);

		normal = -vec3(normColor.r * 2.0f - 1.0f,  normColor.b * 2.0f - 1.0f, (normColor.g * 2.0f - 1.0f));		
	}
	
	calcLightning(color, textureColor, normal, lightVector);
	
	
	gl_FragColor = color; 
}

void calcLightning(out mediump vec4 color, mediump vec4 textureColor, lowp vec3 normal, lowp vec3 lightVector)
{
	color = v_ambientColor;
	if (u_hasTexture > 0) color *= textureColor;	
	
	mediump vec3 L = normalize(lightVector);
	normal = normalize(normal);

	lowp float lambertTerm = dot(normal,L);
	
	if(lambertTerm > 0.0)
	{		

		lowp vec4 addColor = u_light.diffuse * 
		               u_material.diffuse * 
					   lambertTerm;	

		if (u_hasTexture > 0) addColor *= textureColor;
		
		color += addColor;

		/*vec3 E = normalize(v_eyepos);
		vec3 R = reflect(-L, N);
		float specular = pow( max(dot(R, E), 0.0), 
		                 u_material.shininess );
		color += u_light.specular * 
		               u_material.specular * 
					   specular;	*/
	}
}
