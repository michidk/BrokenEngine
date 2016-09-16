#version 420

// transformation matrices
uniform mat4 u_modelViewProjMatrix;
uniform mat4 u_modelWorldMatrix;
uniform mat4 u_normalMatrix;

// vertex attributes
in vec3 v_position;
in vec3 v_normal;
in vec4 v_color;

// shader output
out vec3 f_position;
out vec3 f_normal;
out vec4 f_color;


void main()
{
    vec3 normal = normalize(v_normal);

	vec4 pos4 = vec4(v_position, 1);
    gl_Position = u_modelViewProjMatrix * pos4; // clip space
	
	// convert position & normals to world space
	f_position = vec3(u_modelWorldMatrix * pos4);
	f_normal = vec3(u_normalMatrix * vec4(normal, 0));	
	f_color = v_color;
}