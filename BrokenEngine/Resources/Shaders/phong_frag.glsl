#version 420

// shader variables
uniform vec3 u_cameraPosition;

// material properties
uniform vec4 u_albedo = vec4(1, 0, 1, 1);

uniform vec3 u_lightDirection;
uniform vec4 u_lightColor = vec4(1, 1, 1, 1);
uniform float u_lightIntensity = 1.0;

uniform float u_specularIntensity = 1.0;
uniform float u_specularShininess = 4.0;	// specular exponent: how 'large' the highlight should be

uniform vec4 u_ambientColor = vec4(0, 0, 0, 1);
uniform float u_ambientIntensity = 1.0;

uniform bool blinn = true;					// use blinn hightlighting?

// vertex shader output
in vec3 f_position;	// interpolated world position
in vec3 f_normal;	// interpolated normal
in vec4 f_color;	// interpolated vertex color

// fragment shader output
out vec4 f_fragColor; // first out variable is automatically written to the screen


void main()
{
	// variables
	vec3 normal = normalize(f_normal);

	// diffuse component
	float lightAmount = dot(normal, u_lightDirection);
	vec4 diffuse = max(lightAmount, 0) * u_lightColor;
	
	// specular component
	vec3 viewDir = normalize(u_cameraPosition - f_position);
	
	// fork for blinn highlights
	float specularAmount;
	// fixes phong cutoff issue with point lightning and is cheaper to calculate because no reflection is needed
	if (blinn)
	{
		vec3 halfwayDir = normalize(u_lightDirection + viewDir);
		specularAmount = dot(normal, halfwayDir);
	}
	else
	{
		vec3 reflectDir = reflect(-u_lightDirection, normal);
		specularAmount = dot(viewDir, reflectDir);
	}

	float shininessFactor = blinn ? 3.0 : 1.0;	// shininess should be 2 to 4 times larger when using blinn highlights
	float highlight = pow(max(specularAmount, 0.0), u_specularShininess * shininessFactor);
	vec4 specular = highlight * u_specularIntensity * u_lightColor;
	
	// ambient component
	vec4 ambient = u_ambientColor * u_ambientIntensity;

	// final composition
	vec4 result = (diffuse + specular + ambient) * u_albedo;
	
	// output
	result.a = 1;	// ignore alpha value
	f_fragColor = result;
}