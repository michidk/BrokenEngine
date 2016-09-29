//# Name Cel Shading
//# Author Michael Lohr
//# Description Toon shader

//# Type VERTEX
#version 420

// shader variables
uniform vec3 u_cameraPosition;

// material properties
uniform vec4 u_albedoColor = vec4(1, 0, 1, 1);

uniform vec3 u_lightDirection;
uniform vec4 u_lightColor = vec4(1, 1, 1, 1);
uniform float u_lightIntensity = 1.0;

uniform float u_specularIntensity = 0.75;
uniform float u_specularShininess = 1000.0;	// specular exponent: how 'large' the highlight should be

uniform vec4 u_ambientColor = vec4(0, 0, 0, 1);
uniform float u_ambientIntensity = 1.0;

uniform float u_shades = 5.0f;
uniform float u_outlineThickness = 0.25f;

// vertex shader output
in vec3 f_position;	// interpolated world position
in vec3 f_normal;	// interpolated normal
in vec4 f_color;	// interpolated vertex color

// fragment shader output
out vec4 f_fragColor; // first out variable is automatically written to the screen


float lerp(float from, float to, float value) {
	return from + (to - from) * value;
}

void main()
{
	// variables
	vec3 normal = normalize(f_normal);

	// diffuse component
	float lightAmount = dot(normal, u_lightDirection);
	lightAmount = floor(lightAmount * u_shades) / u_shades;	// limit color shades to u_shades
	vec4 diffuse = max(0.0, lightAmount) * u_lightColor;

	// specular component
	vec3 viewDir = normalize(u_cameraPosition - f_position);
	vec3 reflectDir = reflect(-u_lightDirection, normal);
	float specularAmount = dot(viewDir, reflectDir);
	float highlight = pow(max(0.0, specularAmount), u_specularShininess);
	
	vec4 specular;
	if (highlight > 0.5) {
		specular = u_lightColor;
		specular.a = u_specularIntensity;
	}

	// ambient component
	vec4 ambient = u_ambientColor * u_ambientIntensity;

	// final composition
	vec4 result = (diffuse + ambient) * u_albedoColor;
	
	// alpha blend the specular highlight
	result.rgb = specular.a * specular.rgb + (1.0 - specular.a) * result.rgb;
	
	// outline
	if (abs(dot(viewDir,normal)) < u_outlineThickness)
		result.rgb = vec3(0, 0, 0);

	// output
	result.a = 1;	// ignore alpha value
	f_fragColor = result;
}
