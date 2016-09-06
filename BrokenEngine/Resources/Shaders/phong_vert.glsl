// we are using OpenGL v4.20
#version 420

// transformation matrices
uniform mat4 u_modelViewProjMatrix;
uniform mat4 u_modelWorldMatrix;
//uniform mat4 u_worldViewMatrix;
uniform mat4 u_normalMatrix;

// attributes of the vertex
in vec3 v_position;
in vec3 v_normal;
in vec4 v_color;

// vertex output which is passed (and then interpolated) to the fragment shader
out vec3 f_position;
out vec3 f_normal;
out vec4 f_color;


void main()
{
	// always normalizing these normals
    vec3 normal = normalize(v_normal);

	// position as vector4 ready for matrice multiplication
	vec4 pos4 = vec4(v_position, 1);

	// gl_Position is a special variable of OpenGL that must be set
	// convert position in clip space for the rasterizer
    gl_Position = u_modelViewProjMatrix * pos4;
	
	// convert position & normals in world space, to better calculate lightning
	f_position = vec3(u_modelWorldMatrix * pos4);
	f_normal = vec3(u_normalMatrix * vec4(normal, 0));	
	f_color = v_color;
}