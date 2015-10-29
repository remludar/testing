#version 330 core

uniform sampler2DArray tex;
in vec3 fragTexCoord;

out vec4 finalColor;

void main(){
	finalColor = texture(tex, fragTexCoord);
	//finalColor = vec4(1.0, 0, 0, 1.0);
}