export interface FeedPost {
  id: string;
  author: {
    name: string;
    role: string;
    avatar: string;
    tower?: string;
    apartment?: string;
  };
  createdAt: string; // ISO string
  audience: 'comunidad' | 'torre' | 'apartamento' | 'rol';
  audienceLabel: string;
  title: string;
  content: string;
  attachments?: Array<{
    type: 'image' | 'pdf' | 'link';
    url: string;
    label?: string;
  }>;
  metrics: {
    likes: number;
    celebrates: number;
    important: number;
    comments: number;
  };
  reactions: Array<{
    user: string;
    type: 'like' | 'celebrate' | 'important';
  }>;
  comments: FeedComment[];
  tags?: string[];
}

export interface FeedComment {
  id: string;
  author: {
    name: string;
    role: string;
    avatar: string;
  };
  message: string;
  createdAt: string;
  replyTo?: string;
}
