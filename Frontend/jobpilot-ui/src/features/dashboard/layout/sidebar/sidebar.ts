import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class SidebarComponent {

  collapsed = signal(false);

  menus = [

    {
      title: 'Dashboard',
      icon: '🏠',
      route: '/dashboard'
    },

    {
      title: 'Find Jobs',
      icon: '💼',
      route: '/dashboard/jobs'
    },

    {
      title: 'Auto Apply',
      icon: '⚡',
      route: '/dashboard/auto-apply'
    },

    {
      title: 'My Resume',
      icon: '📄',
      route: '/dashboard/resumes'
    },

    {
      title: 'AI Resume',
      icon: '🤖',
      route: '/dashboard/ai-resume'
    },

    {
      title: 'Applications',
      icon: '📑',
      route: '/dashboard/applications'
    },

    {
      title: 'Saved Jobs',
      icon: '⭐',
      route: '/dashboard/saved'
    },

    {
      title: 'Notifications',
      icon: '🔔',
      route: '/dashboard/notifications'
    },

    {
      title: 'Profile',
      icon: '👤',
      route: '/dashboard/profile'
    },

    {
      title: 'Settings',
      icon: '⚙',
      route: '/dashboard/settings'
    }

  ];

}