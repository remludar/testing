﻿#version 330 core

layout(location=0) in vec4 vPosition;
layout(location=1) in vec3 vertTexCoord;

out vec3 fragTexCoord;

void main(){
	gl_Position = vPosition;
	fragTexCoord = vertTexCoord;
}