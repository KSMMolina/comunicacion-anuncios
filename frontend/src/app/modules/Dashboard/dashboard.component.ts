import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { UserLocalService } from '@core/Application/services/user.service';
import { FeedPost } from './types';
import { Contacts } from "../contacts/contacts";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, Contacts],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  private fb = inject(FormBuilder);
  private userLocal = inject(UserLocalService);

  scopeFilter = signal<'todos' | 'torre' | 'apartamento' | 'rol'>('todos');
  searchTerm = signal('');
  isComposing = signal(false);

  currentUser = computed(() => this.userLocal.user());

  composeForm = this.fb.group({
    title: [''],
    content: ['']
  });

  posts = signal<FeedPost[]>([]);

  filteredPosts = computed(() => {
    const posts = this.posts();
    const scope = this.scopeFilter();
    const query = this.searchTerm().trim().toLowerCase();

    return posts.filter(post => {
      const matchesScope = scope === 'todos' || post.audience === scope;

      const matchesQuery =
        !query ||
        post.title.toLowerCase().includes(query) ||
        post.content.toLowerCase().includes(query) ||
        post.tags?.some(tag => tag.toLowerCase().includes(query));

      return matchesScope && matchesQuery;
    });
  });

  reactions = [
    { type: 'like' as const, label: 'Me gusta', icon: '👍' },
    { type: 'celebrate' as const, label: 'Me encanta', icon: '🎉' },
    { type: 'important' as const, label: 'Importante', icon: '❗' }
  ];

  constructor() {
    this.seedPosts();
  }

  seedPosts() {
    this.posts.set([
      {
        id: '1',
        author: {
          name: 'Andrea Gómez',
          role: 'Administración',
          avatar: 'https://i.pravatar.cc/150?img=47',
          tower: 'Torre A',
        },
        createdAt: new Date().toISOString(),
        audience: 'comunidad',
        audienceLabel: 'Toda la comunidad',
        title: 'Mantenimiento programado de ascensores',
        content:
          'Este jueves 28 se realizará mantenimiento preventivo en los ascensores de la Torre B de 9 a.m. a 12 m. El servicio se reanudará gradualmente. Gracias por su comprensión.',
        attachments: [
          {
            type: 'pdf',
            url: '#',
            label: 'Cronograma de mantenimiento',
          },
        ],
        metrics: {
          likes: 34,
          celebrates: 8,
          important: 21,
          comments: 12,
        },
        reactions: [
          { user: 'César', type: 'important' },
          { user: 'Lucía', type: 'like' },
        ],
        comments: [
          {
            id: 'c1',
            author: {
              name: 'Mariana Rivas',
              role: 'Residente',
              avatar: 'https://i.pravatar.cc/150?img=12',
            },
            message: '¿El servicio se reanudará por completo al mediodía?',
            createdAt: new Date().toISOString(),
          },
        ],
        tags: ['mantenimiento', 'ascensores'],
      },
      {
        id: '2',
        author: {
          name: 'Carlos Pérez',
          role: 'Seguridad',
          avatar: 'https://i.pravatar.cc/150?img=22',
          tower: 'Torre C',
        },
        createdAt: new Date(Date.now() - 3600 * 1000).toISOString(),
        audience: 'torre',
        audienceLabel: 'Torre C',
        title: 'Simulacro de evacuación',
        content:
          'Recordatorio: mañana realizaremos el simulacro de evacuación a las 10:00 a.m. Por favor permanezcan atentos a las indicaciones del personal de seguridad.',
        metrics: {
          likes: 12,
          celebrates: 4,
          important: 16,
          comments: 5,
        },
        reactions: [
          { user: 'Ana', type: 'celebrate' },
          { user: 'Luis', type: 'important' },
        ],
        comments: [],
        tags: ['seguridad', 'simulacro'],
      },
      {
        id: '3',
        author: {
          name: 'Comité de Cultura',
          role: 'Administración',
          avatar: 'https://i.pravatar.cc/150?img=33',
        },
        createdAt: new Date(Date.now() - 7200 * 1000).toISOString(),
        audience: 'comunidad',
        audienceLabel: 'Toda la comunidad',
        title: 'Festival gastronómico este fin de semana',
        content:
          'Este sábado tendremos el festival gastronómico en la terraza central desde las 5:00 p.m. Música en vivo, stands culinarios y actividades para niños. ¡Los esperamos!',
        attachments: [
          {
            type: 'image',
            url: 'https://images.unsplash.com/photo-1555992336-cbf5127451dc?auto=format&fit=crop&w=1200&q=80',
            label: 'Festival edición pasada',
          },
        ],
        metrics: {
          likes: 56,
          celebrates: 32,
          important: 9,
          comments: 18,
        },
        reactions: [
          { user: 'Sofía', type: 'celebrate' },
          { user: 'Elena', type: 'like' },
        ],
        comments: [
          {
            id: 'c2',
            author: {
              name: 'Grupo Jóvenes',
              role: 'Residente',
              avatar: 'https://i.pravatar.cc/150?img=56',
            },
            message: '¿Podemos apoyar con un stand para postres veganos?',
            createdAt: new Date(Date.now() - 3600 * 1000).toISOString(),
          },
        ],
        tags: ['eventos', 'cultura'],
      },
    ]);
  }

  toggleComposer() {
    this.isComposing.update(v => !v);
  }

  submitDraft() {
    const { title, content } = this.composeForm.getRawValue();
    console.log('[Dashboard] Borrador listo', { title, content });
    this.composeForm.reset();
    this.isComposing.set(false);
  }

  updateSearch(value: string) {
    this.searchTerm.set(value);
  }

  changeScope(scope: 'todos' | 'torre' | 'apartamento' | 'rol') {
    this.scopeFilter.set(scope);
  }

  reactTo(postId: string, type: 'like' | 'celebrate' | 'important') {
    console.log(`[Dashboard] reacción ${type} en post ${postId}`);
  }

  commentOn(postId: string) {
    console.log(`[Dashboard] comentar post ${postId}`);
  }

  share(postId: string) {
    console.log(`[Dashboard] compartir post ${postId}`);
  }
}
